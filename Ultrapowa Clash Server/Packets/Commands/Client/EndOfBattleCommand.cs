﻿using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    // Packet 603
    internal class EndOfBattleCommand : Command
    {
        public EndOfBattleCommand(PacketReader br)
        {
        }

        public override void Execute(Level level)
        {
        }
    }
}