using System;
using System.IO;
using System.Text;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing.Messages.Server;
using  UCS.PacketProcessing.Commands.Server;
using System.Threading.Tasks;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14316
    internal class EditClanSettingsMessage : Message
    {
        public EditClanSettingsMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        int m_vAllianceBadgeData;
        string m_vAllianceDescription;
        int m_vAllianceOrigin;
        int m_vAllianceType;
        int m_vRequiredScore;
        int m_vWarFrequency;
        byte m_vWarAndFriendlyStatus;      
        int Unknown;

        public override void Decode()
        {
            using (var br = new PacketReader (new MemoryStream(GetData())))
            {
                m_vAllianceDescription = br.ReadString();
                Unknown = br.ReadInt32();
                m_vAllianceBadgeData = br.ReadInt32();
                m_vAllianceType = br.ReadInt32();
                m_vRequiredScore = br.ReadInt32();
                m_vWarFrequency = br.ReadInt32();
                m_vAllianceOrigin = br.ReadInt32();
                m_vWarAndFriendlyStatus = br.ReadByte();
                //Console.WriteLine(m_vWarAndFriendlyStatus);
            }
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            if (alliance != null)
            {
                alliance.SetAllianceDescription(m_vAllianceDescription);
                alliance.SetAllianceBadgeData(m_vAllianceBadgeData);
                alliance.SetAllianceType(m_vAllianceType);
                alliance.SetRequiredScore(m_vRequiredScore);
                alliance.SetWarFrequency(m_vWarFrequency);
                alliance.SetAllianceOrigin(m_vAllianceOrigin);
                alliance.SetWarAndFriendlytStatus(m_vWarAndFriendlyStatus);

                var avatar = level.GetPlayerAvatar();
                var allianceId = avatar.GetAllianceId();
                var eventStreamEntry = new AllianceEventStreamEntry();
                eventStreamEntry.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                eventStreamEntry.SetSender(avatar);
                eventStreamEntry.SetEventType(10);
                eventStreamEntry.SetAvatarId(avatar.GetId());
                eventStreamEntry.SetAvatarName(avatar.GetAvatarName());
                eventStreamEntry.SetSenderId(avatar.GetId());
                eventStreamEntry.SetSenderName(avatar.GetAvatarName());
                alliance.AddChatMessage(eventStreamEntry);

                var edit = new AllianceSettingChangedCommand();
                edit.SetAlliance(alliance);
                edit.SetPlayer(level);

                var availableServerCommandMessage = new AvailableServerCommandMessage(level.GetClient());
                availableServerCommandMessage.SetCommandId(6);
                availableServerCommandMessage.SetCommand(edit);
                availableServerCommandMessage.Send();

                Parallel.ForEach(alliance.GetAllianceMembers(), op =>
                {
                    var user = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (ResourcesManager.IsPlayerOnline(user))
                    {
                        var p = new AllianceStreamEntryMessage(user.GetClient());
                        p.SetStreamEntry(eventStreamEntry);
                        p.Send();
                    }
                });

                var clan = DatabaseManager.Single().Save(alliance);
                clan.Wait();
            }
        }
    }
}
