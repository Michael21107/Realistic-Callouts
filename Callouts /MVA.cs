using LSPD_First_Response.Engine;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealisticCallouts.callouts
{
    [CalloutInfo("MVA", CalloutProbability.Medium)]
    public class MVA : Callout 
    {
        private String[] Vehicles = new string[] {"NINFEF2", "BUS", "COACH", "AIRBUS", "BARRACKS", "BARRACKS2", "BALLER", "BALLER2", "BANSHEE", "BJXL", "BENSON", "BOBCATXL",
            "BUCCANEER", "BUFFALO", "BUFFALO2", "BULLDOZER", "BULLET", "BURRITO", "BURRITO2", "BURRITO3", "BURRITO4", "BURRITO5", "CAVALCADE", "CAVALCADE2", "GBURRITO",
            "CAMPER", "CARBONIZZARE", "CHEETAH", "COMET2", "COGCABRIO", "COQUETTE", "GRESLEY", "DUNE2", "HOTKNIFE", "DUBSTA", "DUBSTA2", "DUMP", "DOMINATOR", "EMPEROR", "EMPEROR2",
            "EMPEROR3", "ENTITYXF", "EXEMPLAR", "ELEGY2", "F620", "FELON", "FELON2", "FELTZER2", "FIRETRUK", "FQ2", "FUGITIVE", "FUTO", "GRANGER", "GAUNTLET", "HABANERO",
            "INFERNUS", "INTRUDER", "JACKAL", "JOURNEY", "JB700", "KHAMELION", "LANDSTALKER", "MESA", "MESA2", "MESA3", "MIXER", "MINIVAN", "MIXER2", "MULE", "MULE2", "ORACLE", "ORACLE2",
            "MONROE", "PATRIOT", "PBUS", "PACKER", "PENUMBRA", "PEYOTE", "PHANTOM", "PHOENIX", "PICADOR", "POUNDER", "PRIMO","RANCHERXL", "RANCHERXL2", "RAPIDGT", "RAPIDGT2", "RENTALBUS",
            "RUINER", "RIPLEY", "SABREGT", "SADLER", "SADLER2", "SANDKING", "SANDKING2","SPEEDO", "SPEEDO2", "STINGER", "STOCKADE", "STINGERGT", "SUPERD", "STRATUM", "SULTAN", "AKUMA"};

        private string[] PedList = new string[] {"u_m_o_finguru_01", "a_f_y_fitness_01","a_f_m_eastsa_01", "cs_drfriedlander", "cs_denise", "a_f_y_hipster_02", "a_m_y_hipster_03",
            "a_m_m_fatlatin_01", "s_f_y_hooker_01", "a_f_y_hipster_04", "u_f_y_hotposh_01", "cs_janet", "a_m_y_jetski_01", "player_zero", "a_m_o_tramp_01",
            "a_m_m_trampbeac_01"," ig_wade" };

        private Ped Driver;
        private Vehicle Vehicle;
        private Blip EventBlip;
        private Blip VehicleBlip;
        private Blip DriverBlip;
        private Vector3 SpawnPoint;
        private bool DriverMarked;
        private bool EventCreated;


        public int RandomNumber(int min, int max)
        {
            int random = new Random().Next(min, max);
            return random;
        }


        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            AddMinimumDistanceCheck(300f, SpawnPoint);
            CalloutMessage = "We Have A MVA Respond ~y~Code 3";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT_01 IN_OR_ON_POSITION", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Vehicle = new Vehicle(Vehicles[new Random().Next((int)Vehicles.Length)], SpawnPoint);

            if (Vehicle.Model.IsBus)
                Driver = new Ped(PedList[new Random().Next((int)PedList.Length)], SpawnPoint, 0f);
            else if (Vehicle.Model.IsBigVehicle)
                Driver = new Ped(PedList[new Random().Next((int)PedList.Length)], SpawnPoint, 0f);
            else

            Driver = new Ped(Vehicle.GetOffsetPositionRight(5f));
            Driver.IsPersistent = true;
            Driver.BlockPermanentEvents = true;
            Driver.WarpIntoVehicle(Vehicle, -1);
            Driver.Health = RandomNumber(100, 200);

            EventBlip = new Blip(SpawnPoint);
            EventBlip.Color = System.Drawing.Color.Orange;
            EventBlip.IsRouteEnabled = true;
            EventBlip.Name = "MVA";

            EventCreated = true;
            DriverMarked = true;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (!EventCreated && Game.LocalPlayer.Character.DistanceTo(Vehicle) < 300f)
            {
                Game.DisplayNotification("The involved vehicle is a ~o~" + Vehicle.Model.Name);

                Vehicle.EngineHealth = RandomNumber(0, 100);
                Vehicle.IsDriveable = false;
                Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;

                if (RandomNumber(0, 2) == 1)
                    Vehicle.Wheels[RandomNumber(0, 2)].BurstTire();

                if (!Vehicle.IsOnScreen)
                    Vehicle.Velocity = new Vector3(20, 30, 0);

                if (RandomNumber(0, 3) == 1)
                    Vehicle.Doors[0].BreakOff();

                if (RandomNumber(0, 3) == 1)
                    Vehicle.PunctureFuelTank();

                if (Driver.IsAlive)
                {
                    Game.LogTrivial("Driver is alive");

                    Driver.Tasks.LeaveVehicle(Vehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion();
                 

                    EventBlip.Delete();
                    VehicleBlip = Vehicle.AttachBlip();
                    VehicleBlip.RouteColor = System.Drawing.Color.Orange;
                    VehicleBlip.Sprite = BlipSprite.VehicleDeathmatch;
                    VehicleBlip.Name = "MVA";
                    VehicleBlip.IsRouteEnabled = true;
                    VehicleBlip.Order = 1;
                    if (Vehicle.IsOnFire)
                    {
                        Game.LogTrivial("Vehicle on fire");
                        if (RandomNumber(0, 2) == 1)
                        {
                            Game.LogTrivial("Driver set on fire");
                            Driver.IsOnFire = false;
                        }
                        else
                        {
                            Game.LogTrivial("Driver fleeing");
                            Driver.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
                            GameFiber.Wait(5000);
                        }
                    }
                    else
                    {
                        if (RandomNumber(0, 2) == 1)
                        {
                            Game.LogTrivial("Driver ragdolling");
                            Driver.IsRagdoll = true;
                        }
                        else
                        {
                            Game.LogTrivial("Driver walking away");
                            Driver.Tasks.Wander();
                            GameFiber.Wait(3000);
                        }
                    }
                    Driver.Tasks.Clear();
                }

                EventCreated = true;
            }

            if (EventCreated && !DriverMarked && (Game.LocalPlayer.Character.DistanceTo(Vehicle) < 20f || Game.LocalPlayer.Character.DistanceTo(Driver) < 20f))
            {
                Game.DisplayNotification("driver");

                Driver.Tasks.LeaveVehicle(Vehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion();

                DriverBlip = Driver.AttachBlip();
                DriverBlip.Order = 2;
                DriverBlip.Scale = 0.8f;
                DriverBlip.Name = "Driver";
                DriverBlip.Color = System.Drawing.Color.LightSkyBlue;
                DriverBlip.IsRouteEnabled = true;

                DriverMarked = true;
            }

            if (EventCreated && DriverMarked && Driver.IsRagdoll && Game.LocalPlayer.Character.DistanceTo(Driver) < 3f)
                Game.DisplaySubtitle("The driver seems to be ~y~Unconscious");


            if (EventCreated && Driver.IsDead)
            {
                Game.DisplayNotification("The driver is dead.");
                Game.DisplayHelp("Press ~b~End~w~ to end the callout.");
            }
            else if (EventCreated && (Game.LocalPlayer.Character.DistanceTo(Driver) > 500f || !Driver.Exists() || Game.LocalPlayer.Character.DistanceTo(Vehicle) > 500f))
            {
                End();
            }
            if (Game.IsKeyDown(System.Windows.Forms.Keys.End))
                End();
        }

        public override void End()
        {
            base.End();

            if (EventBlip.Exists()) EventBlip.Delete();
            if (DriverBlip.Exists()) DriverBlip.Delete();
            if (Driver.Exists())
            {
                if (Driver.Tasks != null) Driver.Tasks.Clear();
                Driver.Dismiss();
            }
            if (VehicleBlip.Exists()) VehicleBlip.Delete();
            if (Vehicle.Exists()) Vehicle.Dismiss();

            Game.DisplayNotification("[Realistic Callouts 'MVA' ENDED]");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        }
    }
}
