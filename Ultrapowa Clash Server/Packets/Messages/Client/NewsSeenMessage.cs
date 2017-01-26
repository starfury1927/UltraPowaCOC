using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 10905
    internal class NewsSeenMessage : Message
    {
        public int unknown { get; set;}

        public NewsSeenMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {

        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                /*
                unknown = br.ReadInt32();

                Console.WriteLine(unknown);  
                */     
            }
        }

        public override void Process(Level level)
        {

        }
    }
}
