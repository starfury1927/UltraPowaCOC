using System;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 700
    internal class SearchOpponentCommand : Command
    {
        public SearchOpponentCommand(PacketReader br)
        {
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {                 
            for (int i = 0; i < 31; i++)
            {
                var unitData = CSVManager.DataTables.GetDataById(4000000 + i);
                var combatData = (CharacterData)unitData;
                var maxLevel = combatData.GetUpgradeLevelCount();
                var unitSlot = new DataSlot(unitData, 6969);

                level.GetPlayerAvatar().GetUnits().Add(unitSlot);
                level.GetPlayerAvatar().SetUnitUpgradeLevel(combatData, maxLevel - 1);
            }

            for (int i = 0; i < 18; i++)
            {
                var spellData = CSVManager.DataTables.GetDataById(26000000 + i);
                var combatData = (SpellData)spellData;
                var maxLevel = combatData.GetUpgradeLevelCount();
                var spellSlot = new DataSlot(spellData, 6969);

                level.GetPlayerAvatar().GetSpells().Add(spellSlot); 
                level.GetPlayerAvatar().SetUnitUpgradeLevel(combatData, maxLevel - 1);
            }
            
            var l = ObjectManager.GetRandomOnlinePlayer();
            if (l != null)
            {
                l.Tick();
                level.GetPlayerAvatar().State = ClientAvatar.UserState.Searching;

                var trophyDiff = Math.Abs(level.GetPlayerAvatar().GetScore() - l.GetPlayerAvatar().GetScore());
                var reward = (int)Math.Round(Math.Pow((5 * trophyDiff), 0.25) + 5d);
                var lost = (int)Math.Round(Math.Pow((2 * trophyDiff), 0.35) + 5d);

                var info = new ClientAvatar.AttackInfo
                {
                    Attacker = level,
                    Defender = l,

                    Lost = lost,
                    Reward = reward,
                    UsedTroop = new List<DataSlot>()
                    
                };

                level.GetPlayerAvatar().AttackingInfo.Add(l.GetPlayerAvatar().GetId(), info); //Use UserID For a while..Working on AttackID soon

                l.Tick();
                new EnemyHomeDataMessage(level.GetClient(), l, level).Send();
            }
           else
                new EnemyHomeDataMessage(level.GetClient(), l, level).Send();
        }
    }
}
