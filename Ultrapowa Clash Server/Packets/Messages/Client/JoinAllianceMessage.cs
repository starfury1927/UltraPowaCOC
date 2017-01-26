using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Commands.Server;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14305
    internal class JoinAllianceMessage : Message
    {
        long m_vAllianceId;

        public JoinAllianceMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                m_vAllianceId = br.ReadInt64WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance != null)
            {
                if (!alliance.IsAllianceFull())
                {
                    level.GetPlayerAvatar().SetAllianceId(alliance.GetAllianceId());
                    var member = new AllianceMemberEntry(level.GetPlayerAvatar().GetId());
                    member.SetRole(1);
                    alliance.AddAllianceMember(member);

                    var b = new JoinedAllianceCommand();
                    b.SetAlliance(alliance);

                    var c = new AllianceRoleUpdateCommand();
                    c.SetAlliance(alliance);
                    c.SetRole(1);
                    c.Tick(level);

                    var a = new AvailableServerCommandMessage(Client);
                    a.SetCommandId(1);
                    a.SetCommand(b);

                    var d = new AvailableServerCommandMessage(Client);
                    d.SetCommandId(8);
                    d.SetCommand(c);

                    a.Send();
                    d.Send();
                    
                     new AllianceStreamMessage(Client, alliance).Send();
                }
            }
        }
    }
}
