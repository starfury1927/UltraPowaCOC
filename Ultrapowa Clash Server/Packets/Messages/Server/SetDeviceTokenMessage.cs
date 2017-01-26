using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 20113
    internal class SetDeviceTokenMessage : Message
    {
        readonly Level level;

        public SetDeviceTokenMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(20113);
            level = client.GetLevel();
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(level.GetPlayerAvatar().GetUserToken());
            Encrypt(pack.ToArray());
        }
    }
}