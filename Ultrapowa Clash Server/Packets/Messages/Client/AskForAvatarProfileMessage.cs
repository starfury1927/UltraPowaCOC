using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14325
    internal class AskForAvatarProfileMessage : Message
    {
        public AskForAvatarProfileMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        long m_vAvatarId;
        long m_vCurrentHomeId;

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                m_vAvatarId = br.ReadInt64();
                br.ReadByte();
                m_vCurrentHomeId = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            var targetLevel = ResourcesManager.GetPlayer(m_vAvatarId);
            if (targetLevel != null)
            {
                targetLevel.Tick();
                var p = new AvatarProfileMessage(Client);
                p.SetLevel(targetLevel);
                p.Send();
            }
        }
    }
}
