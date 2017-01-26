using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace  UCS.PacketProcessing.Commands
{
    internal class ChallangeCommand : Command
    {
        public string Message { get; set; }

        public ChallangeCommand(PacketReader br)
        {
            Message = br.ReadString();
        }

        public override void Execute(Level level)
        {
            var player = level.GetPlayerAvatar();
            var allianceID = player.GetAllianceId();
            var alliance = ObjectManager.GetAlliance(allianceID);

            var cm = new ChallangeStreamEntry();
            cm.SetMessage(Message);
            cm.SetSenderId(player.GetId());
            cm.SetSenderName(player.GetAvatarName());
            cm.SetSenderLevel(player.GetAvatarLevel());
            cm.SetSenderRole(player.GetAllianceRole());
            cm.SetId(alliance.GetChatMessages().Count + 1);
            cm.SetSenderLeagueId(player.GetLeagueId());

            var s = alliance.GetChatMessages().Find(c => c.GetStreamEntryType() == 12);
            if (s != null)
            {
                alliance.GetChatMessages().RemoveAll(t => t == s);
                Parallel.ForEach ((alliance.GetAllianceMembers()), op =>
                {
                    var alliancemembers = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (alliancemembers.GetClient() != null)
                    {
                        new AllianceStreamEntryRemovedMessage(alliancemembers.GetClient(), s.GetId()).Send();
                    }
                });
            }

            alliance.AddChatMessage(cm);

            Parallel.ForEach((alliance.GetAllianceMembers()), op =>
            {
                var alliancemembers = ResourcesManager.GetPlayer(op.GetAvatarId());
                if (alliancemembers.GetClient() != null)
                {
                    var p = new AllianceStreamEntryMessage(alliancemembers.GetClient());
                    p.SetStreamEntry(cm);
                    p.Send();
                }
            });
        }
    }
}
