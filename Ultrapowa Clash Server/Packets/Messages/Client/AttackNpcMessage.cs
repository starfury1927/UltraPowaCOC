using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14134
    internal class AttackNpcMessage : Message
    {
        public AttackNpcMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public int LevelId { get; set; }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                LevelId = br.ReadInt32WithEndian();
            }
        }

        public override void Process(Level level)
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

            for (int i = 0; i < 19; i++)
            {
                var spellData = CSVManager.DataTables.GetDataById(26000000 + i);
                var combatData = (SpellData)spellData;
                var maxLevel = combatData.GetUpgradeLevelCount();
                var spellSlot = new DataSlot(spellData, 6969);

                level.GetPlayerAvatar().GetSpells().Add(spellSlot);
                level.GetPlayerAvatar().SetUnitUpgradeLevel(combatData, maxLevel - 1);
            }

            new NpcDataMessage(Client, level, this).Send();
        }
    }
}
