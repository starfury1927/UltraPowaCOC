﻿using System.Collections.Generic;
using System.IO;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 526
    internal class BoostBuildingCommand : Command
    {
        public BoostBuildingCommand(PacketReader br)
        {
            BuildingIds = new List<int>();
            BoostedBuildingsCount = br.ReadInt32WithEndian();
            for (var i = 0; i < BoostedBuildingsCount; i++)
            {
                BuildingIds.Add(br.ReadInt32WithEndian());
            }
        }

        public override void Execute(Level level)
        {
            var ca = level.GetPlayerAvatar();
            foreach (var buildingId in BuildingIds)
            {
                var go = level.GameObjectManager.GetGameObjectByID(buildingId);
                var b = (ConstructionItem) go;
                var costs = ((BuildingData) b.GetConstructionItemData()).BoostCost[b.UpgradeLevel];
                if (ca.HasEnoughDiamonds(costs))
                {
                    b.BoostBuilding();
                    ca.SetDiamonds(ca.GetDiamonds() - costs);
                }
            }
        }

        public int BoostedBuildingsCount { get; set; }
        public List<int> BuildingIds { get; set; }
    }
}