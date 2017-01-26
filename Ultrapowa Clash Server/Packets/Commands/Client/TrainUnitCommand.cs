using System;
using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    // Packet 508
    internal class TrainUnitCommand : Command
    {
        public TrainUnitCommand(PacketReader br)
        {
            br.ReadInt32WithEndian();
            br.ReadUInt32WithEndian();
            UnitType = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
            br.ReadUInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        public int Count { get; set; }
        public int UnitType { get; set; }

        public override void Execute(Level level)
        {
            var troopdata = (CombatItemData)CSVManager.DataTables.GetDataById(UnitType);    
            //TODO
        }
    }
}
