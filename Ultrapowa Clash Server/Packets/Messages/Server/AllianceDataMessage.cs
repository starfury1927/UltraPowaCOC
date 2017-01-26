using System.Collections.Generic;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 24301
    internal class AllianceDataMessage : Message
    {
        readonly Alliance m_vAlliance;

        public AllianceDataMessage(PacketProcessing.Client client, Alliance alliance) : base(client)
        {
            SetMessageType(24301);
            m_vAlliance = alliance;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            var allianceMembers = m_vAlliance.GetAllianceMembers();

            pack.AddRange(m_vAlliance.EncodeFullEntry());
            pack.AddString(m_vAlliance.GetAllianceDescription());
            pack.AddInt32(6);
            pack.Add(0);  
            pack.AddInt32(0);
            pack.Add(0);

            pack.AddInt32(allianceMembers.Count);

            foreach(var m in allianceMembers)
            {
                pack.AddRange(m.Encode());
            }

            pack.AddInt32(0);
            pack.AddInt32(32);
            Encrypt(pack.ToArray());
        }
    }
}
