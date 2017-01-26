using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using UCS.Core.Threading;
using UCS.Core.Web;
using static System.Console;
using static UCS.Core.Logger;

namespace UCS.Core.Settings
{
    internal class UCSControl
    {
        public static void UCSClose()
        {
            Thread T = new Thread(() =>
            {
                Say("Closing UCS...");
                NetworkThread.Stop();
                MemoryThread.Stop();
                Environment.Exit(0);
            }); T.Start();
        }

        public static void UCSRestart()
        {
            Thread T = new Thread(() =>
            {
                Say("Restarting UCS...");
		        NetworkThread.Stop();
                MemoryThread.Stop();  
		        Thread.Sleep(1000);
                Process.Start("UCS.exe");
		        Environment.Exit(0);   
            }); T.Start();
        }

        public static void UpdateTitle(bool Status)
        {
            if (Status == false)
            {
                Console.Title = Constants.DefaultTitle + "OFFLINE";
            }
            else if (Status == true)
            {
                Console.Title = Constants.DefaultTitle + "ONLINE";
            }
        }

        public static void UpdateGuiStatus()
        {
            if (Console.Title.Contains("ONLINE"))
            {
                UCSUI Gui = (UCSUI)Application.OpenForms["UCSUI"];
                Gui.labelOnlineStatus.Text = "ONLINE";
            }
        }

        public static void WelcomeMessage()
        {
            UpdateTitle(false);
            ForegroundColor = ConsoleColor.Red;
            WriteLine(

                @"
      ____ ___.__   __                                              
     |    |   \  |_/  |_____________  ______   ______  _  _______   
     |    |   /  |\   __\_  __ \__  \ \____ \ /  _ \ \/ \/ /\__  \  
     |    |  /|  |_|  |  |  | \// __ \|  |_> >  <_> )     /  / __ \_
     |______/ |____/__|  |__|  (____  /   __/ \____/ \/\_/  (____  /
                                    \/|__|                       \/
            ");
            ResetColor();
            WriteLine("[UCS]    > This program is made by the Ultrapowa Development Team.\n[UCS]    > Ultrapowa is not affiliated to \"Supercell, Oy\".\n[UCS]    > UCS is proudly licensed under the Apache License, Version 2.0.\n[UCS]    > Visit www.ultrapowa.com daily for News & Updates!");
            if (Constants.IsRc4)
                WriteLine("[UCS]    > UCS is running under RC4 mode. Please make sure CSV is modded to allow RC4 client to succesfuly connect");
            else
                WriteLine("[UCS]    > UCS is running under Pepper mode. Please make sure client key is modded to allow Pepper client to succesfuly connect");
            Console.Write("[UCS]    ");
            ForegroundColor = ConsoleColor.Red;
            WriteLine("> UCS is up-to-date: " + Constants.Version);
            ResetColor();
            WriteLine("\n[UCS]    Prepearing Server...\n");
        }
    }
}
