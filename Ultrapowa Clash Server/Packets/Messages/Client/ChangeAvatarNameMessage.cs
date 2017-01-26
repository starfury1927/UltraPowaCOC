using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 10212
    internal class ChangeAvatarNameMessage : Message
    {
        public ChangeAvatarNameMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        string PlayerName { get; set; }  

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                PlayerName = br.ReadScString();
            }
        }

        public override void Process(Level level)
        {
            level.GetPlayerAvatar().SetName(PlayerName);
            var p = new AvatarNameChangeOkMessage(Client);
            p.SetAvatarName(level.GetPlayerAvatar().GetAvatarName());
            p.Send();
        }
    }
}
