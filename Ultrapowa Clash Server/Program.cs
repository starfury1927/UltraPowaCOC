using System.Threading;
using UCS.Core.Settings;
using UCS.Core.Threading;
using UCS.Helpers;

namespace UCS
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Thread T = new Thread(() =>
            {
                UCSControl.WelcomeMessage();
				CheckThread.Start();
                MemoryThread.Start();
                NetworkThread.Start();
                ParserThread.Start();
            }); T.Start();
            T.Priority = ThreadPriority.Highest;
        }
    }
}
