using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;
using UCS.PacketProcessing.Commands.Client;
using System.Text;
using static UCS.Logic.ClientAvatar;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14101
    internal class GoHomeMessage : Message
    {
        public GoHomeMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                State = br.ReadInt32WithEndian();              
            }
        }

        public int State { get; set; }

        public override void Process(Level level)
        {
            if (level.GetPlayerAvatar().State == UserState.PVP)
            {
                var info = default(ClientAvatar.AttackInfo);
                if (!level.GetPlayerAvatar().AttackingInfo.TryGetValue(level.GetPlayerAvatar().GetId(), out info))
                {
                    Logger.Write("Unable to obtain attack info.");
                }
                else
                {
                    var defender = info.Defender;
                    var attacker = info.Attacker;

                    var lost = info.Lost;
                    var reward = info.Reward;

                    var usedtroop = info.UsedTroop;

                    int attackerscore = attacker.GetPlayerAvatar().GetScore();
                    int defenderscore = defender.GetPlayerAvatar().GetScore();

                    if (defender.GetPlayerAvatar().GetScore() > 0)
                        defender.GetPlayerAvatar().SetScore(defenderscore -= lost);

                    Logger.Write("Used troop type: " + usedtroop.Count);
                    foreach(var a in usedtroop)
                    {
                        Logger.Write("Troop Name: " + a.Data.GetName());
                        Logger.Write("Troop Used Value: " + a.Value);
                    }
                    attacker.GetPlayerAvatar().SetScore(attackerscore += reward);
                    attacker.GetPlayerAvatar().AttackingInfo.Clear(); //Since we use userid for now,We need to clear to prevent overlapping
                    Resources(attacker);

                    var attackerdb = DatabaseManager.Single().Save(attacker);
                    attackerdb.Wait();
                    var defenderdb = DatabaseManager.Single().Save(defender);
                    defenderdb.Wait();

                }
            }
            if (level.GetPlayerAvatar().State == UserState.CHA)
            {
                //Attack 
            }

            if (State == 1)
            {
                var player = level.GetPlayerAvatar();
                player.State = UserState.Editmode;
            }
            else
            {
                var player = level.GetPlayerAvatar();
                player.State = UserState.Home;
            }

            level.Tick();
            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            new OwnHomeDataMessage(Client, level).Send();
            if (alliance != null)
            {
                new AllianceStreamMessage(Client, alliance).Send(); 
            }
        }

        public void Resources(Level level)
        {
            var avatar = level.GetPlayerAvatar();
            var currentGold = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("Gold"));
            var currentElixir = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("Elixir"));
            var goldLocation = CSVManager.DataTables.GetResourceByName("Gold");
            var elixirLocation = CSVManager.DataTables.GetResourceByName("Elixir");

            if (currentGold >= 1000000000 | currentElixir >= 1000000000)
            {
                avatar.SetResourceCount(goldLocation, currentGold + 10);
                avatar.SetResourceCount(elixirLocation, currentElixir + 10);
            }
            else if (currentGold <= 999999999 || currentElixir <= 999999999)
            {
                avatar.SetResourceCount(goldLocation, currentGold + 1000);
                avatar.SetResourceCount(elixirLocation, currentElixir + 1000);
            }
        } 
    }
}
