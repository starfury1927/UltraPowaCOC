using System.Threading;
using UCS.Core.Checker;
using UCS.Core.Web;

namespace UCS.Core.Threading
{
    internal class CheckThread
    {
        public static void Start()
        {
            Thread T = new Thread(() =>
            {
                //LicenseChecker.CheckForSavedKey(); //disabled atm
                VersionChecker.VersionMain();
                DirectoryChecker.Directorys();
                DirectoryChecker.Files();
            }); T.Start();
            T.Priority = ThreadPriority.Normal; 
        }
    }
}
