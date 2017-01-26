using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 514
    internal class SpeedUpClearingCommand : Command
    {
        readonly int m_vObstacleId;

        public SpeedUpClearingCommand(PacketReader br)
        {
            m_vObstacleId = br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {
        }
    }
}
