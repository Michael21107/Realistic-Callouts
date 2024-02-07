using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using RealisticCallouts.callouts;
using System.Runtime;
using LSPD_First_Response.Mod.Callouts;

namespace RealisticCallouts
{
    public class Main : Plugin
    {
        public static bool CalloutInterface { get; internal set; }

        public override void Finally() { }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }
        static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
                GameFiber.StartNew(delegate
                {
                    RegisterCallouts();
                    Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                    Game.Console.Print("[LOG]: The config file was loaded successfully.");
                    Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "RealisticCallouts", "~y~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~o~by Tyler.M", "~b~successfully loaded Enjoy!");
                });
        }
        private static void RegisterCallouts() //Register all your callouts here
        {
            Functions.RegisterCallout(typeof(callouts.HighSpeedChase));
            //Functions.RegisterCallout(typeof(callouts.IntoxicatedIndividuals));
            Functions.RegisterCallout(typeof(callouts.PossableStolenVehicle));
            Functions.RegisterCallout(typeof(callouts.MVA));

        }
    }
}
