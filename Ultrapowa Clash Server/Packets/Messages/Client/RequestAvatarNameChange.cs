using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14600
    internal class RequestAvatarNameChange : Message
    {
        public RequestAvatarNameChange(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public string PlayerName { get; set; }

        public byte Unknown1 { get; set; }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                PlayerName = br.ReadString();
            }
        }

        public override void Process(Level level)
        {
            var id = level.GetPlayerAvatar().GetId();
            var l = ResourcesManager.GetPlayer(id);
            if (l != null)
            {
                l.GetPlayerAvatar().SetName(PlayerName);
                var p = new AvatarNameChangeOkMessage(l.GetClient());
                p.SetAvatarName(PlayerName);
                p.Send();
            }
        }
    }
}