using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14113
    internal class VisitHomeMessage : Message
    {
        public VisitHomeMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public long AvatarId { get; set; }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                AvatarId = br.ReadInt64WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var targetLevel = ResourcesManager.GetPlayer(AvatarId);
            targetLevel.Tick();
            var clan = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            new VisitedHomeDataMessage(Client, targetLevel, level).Send();
            if (clan != null)
                new AllianceStreamMessage(Client, clan).Send();
        }
    }
}
