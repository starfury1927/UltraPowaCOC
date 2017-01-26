using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UCS.Core.Settings;
using UCS.PacketProcessing;
using ThreadState = System.Threading.ThreadState;
using Timer = System.Timers.Timer;

namespace UCS.Core.Threading
{
    internal class MemoryThread
    {
        static Thread T { get; set; }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize,
            UIntPtr maximumWorkingSetSize);

        public static void Start()
        {
            T = new Thread(() =>
            {
                var t = new Timer();
                t.Interval = Constants.MemoryInterval;
                t.Elapsed += (s, a) =>
                {
                    foreach (Client p in ResourcesManager.GetConnectedClients())
                    {
                        if (!p.IsClientSocketConnected())
                            ResourcesManager.DropClient(p.GetSocketHandle());
                    }

                    GC.Collect(GC.MaxGeneration);
                    GC.WaitForPendingFinalizers();
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)0xFFFFFFFF,
                    (UIntPtr)0xFFFFFFFF);
                };
                t.Enabled = true;
            }); T.Start();
            T.Priority = ThreadPriority.Normal;
        }
        public static void Stop()
        {
            T.Abort();
        }
    }
}
