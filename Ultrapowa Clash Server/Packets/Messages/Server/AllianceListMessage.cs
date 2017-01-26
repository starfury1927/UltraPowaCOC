using System.Collections.Generic;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 24310
    internal class AllianceListMessage : Message
    {
        public AllianceListMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24310);
            m_vAlliances = new List<Alliance>();
        }

        List<Alliance> m_vAlliances;
        string m_vSearchString;

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(m_vSearchString);
            pack.AddInt32(m_vAlliances.Count);
            Parallel.ForEach((m_vAlliances), (alliance, l) =>
            {
                pack.AddRange(alliance.EncodeFullEntry());
                l.Stop();
            });
            Encrypt(pack.ToArray());
        }

        public void SetAlliances(List<Alliance> alliances)
        {
            m_vAlliances = alliances;
        }

        public void SetSearchString(string searchString)
        {
            m_vSearchString = searchString;
        }
    }
}
