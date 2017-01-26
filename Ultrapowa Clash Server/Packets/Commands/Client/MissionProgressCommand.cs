using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 519
    internal class MissionProgressCommand : Command
    {
        public MissionProgressCommand(PacketReader br)
        {
        }

        public uint MissionId { get; set; }
        public uint Unknown1 { get; set; }
    }
}