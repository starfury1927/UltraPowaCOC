using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 538
    internal class MyLeagueCommand : Command
    {
        public MyLeagueCommand(PacketReader br)
        {

        }

        public override void Execute(Level level)
        {
            //PacketManager.ProcessOutgoingPacket(new LeaguePlayersMessage(level.GetClient()));
        }
    }
}
