using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 507
    internal class ClearObstacleCommand : Command
    {
        public ClearObstacleCommand(PacketReader br)
        {
            ObstacleId = br.ReadInt32WithEndian();
            Tick = br.ReadUInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            var player = level.GetPlayerAvatar();
            var ob = level.GameObjectManager.GetGameObjectByID(ObstacleId);
			if (ObstacleId != null)
			{
				level.GameObjectManager.RemoveGameObject(ob);
			}            
        }

        public int ObstacleId { get; set; }
        public uint Tick { get; set; }
    }
}
