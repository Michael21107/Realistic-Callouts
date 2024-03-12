using LSPD_First_Response.Engine;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealisticCallouts.callouts
{
    [CalloutInfo("Possable Stolen Vehicle", CalloutProbability.High)]
    public class PossableStolenVehicle : Callout
    {
        private String[] StolenVehicles = new String[] {"NINFEF2", "BUS", "COACH", "BALLER", "BALLER2", "BANSHEE", "BJXL", "BENSON", "BOBCATXL",
            "BUCCANEER", "BUFFALO", "BUFFALO2", "BULLET", "BURRITO", "BURRITO2", "BURRITO3", "BURRITO4", "CAVALCADE", "CAVALCADE2", "GBURRITO",
            "CAMPER", "CARBONIZZARE", "CHEETAH", "COMET2", "COGCABRIO", "COQUETTE", "GRESLEY", "HOTKNIFE", "DUBSTA", "DUBSTA2", "DOMINATOR", "EMPEROR", "EMPEROR2",
            "EMPEROR3", "ENTITYXF", "EXEMPLAR", "ELEGY2", "F620", "FELON", "FELON2", "FELTZER2", "FIRETRUK", "FQ2", "FUGITIVE", "FUTO", "GRANGER", "GAUNTLET", "HABANERO",
            "INFERNUS", "INTRUDER", "JACKAL", "JB700", "KHAMELION", "LANDSTALKER", "MESA", "MESA2", "MESA3", "MIXER", "MINIVAN", "MIXER2", "MULE", "MULE2", "ORACLE", "ORACLE2",
            "MONROE", "PATRIOT", "PBUS", "PACKER", "PENUMBRA", "PEYOTE", "PHANTOM", "PHOENIX", "PICADOR", "POUNDER", "PRIMO","RANCHERXL", "RANCHERXL2", "RAPIDGT", "RAPIDGT2", "RENTALBUS",
            "RUINER", "RIPLEY", "SABREGT", "SADLER", "SADLER2", "SANDKING", "SANDKING2","SPEEDO", "SPEEDO2", "STINGER", "STOCKADE", "STINGERGT", "SUPERD", "STRATUM", "SULTAN", "AKUMA"};

        private Vector3 Spawnpoint;
        private Ped player = Game.LocalPlayer.Character;
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vehicle StolenVehicle;
        private LHandle MainPursuit;
        private object Weapons;
        private bool PursuitCreated;
        private LHandle Pursuit;

        public int RandomNumber(int min, int max)
        {
            int random = new Random().Next(min, max);
            return random;
        }

        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            AddMinimumDistanceCheck(300f, Spawnpoint);
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            CalloutMessage = "We Have A High Speed Chase Respond ~w~Code 3kjih.";
            CalloutPosition = Spawnpoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", Spawnpoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("RealisticCallouts Log: High Speed Chase.");

            StolenVehicle = new Vehicle(StolenVehicles[new Random().Next((int)StolenVehicles.Length)], Spawnpoint);

            Suspect = new Ped(Spawnpoint);
            Suspect.WarpIntoVehicle(StolenVehicle, -1);
            Suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();

            SuspectBlip.IsRouteEnabled = false;
            SuspectBlip.Color = System.Drawing.Color.Red;
            Pursuit = Functions.CreatePursuit();
            Functions.AddPedToPursuit(Pursuit, Suspect);
            Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
            PursuitCreated = true;

            System.Random rng = new System.Random();

            return base.OnCalloutAccepted();
        }
        public override void OnCalloutNotAccepted()
        {
            Game.LogTrivial("First Callout Not Accepted by User."); 

            base.OnCalloutNotAccepted();
        }
        private void Callout()
        {
            GameFiber.StartNew(delegate
            {
                while (player.DistanceTo(Suspect) > 20) GameFiber.Wait(0);

                Game.DisplaySubtitle("Dispatch, we are ~b~on Scene!", 2500);
                Suspect.Tasks.LeaveVehicle(Suspect.CurrentVehicle, LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion(); 
                Suspect.Tasks.FightAgainst(player, 5000).WaitForCompletion();
                Suspect.Tasks.EnterVehicle(StolenVehicle, -1).WaitForCompletion();

                GameFiber.Wait(2000);

                Game.LogTrivial("Suspect Pursuit Event Started.");

                if (SuspectBlip.Exists()) SuspectBlip.IsRouteEnabled = false;

                MainPursuit = Functions.CreatePursuit();

                Functions.PlayScannerAudio("CRIME_SUSPECT_ON_THE_RUN_01");

                Game.DisplayNotification("Suspect is ~r~Evading!");

                try 
                {
                    Functions.RequestBackup(Spawnpoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                    Functions.RequestBackup(Spawnpoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit);
                }
                catch 
                {

                }
                Functions.SetPursuitIsActiveForPlayer(MainPursuit, true);
                Functions.AddPedToPursuit(MainPursuit, Suspect);

                while (Functions.IsPursuitStillRunning(MainPursuit)) GameFiber.Wait(0);

                if (Suspect.Exists())
                { 
                    if (Suspect.IsDead) Game.DisplayNotification("Dispatch, Suspect is ~r~Dead.");
                    else Game.DisplayNotification("Dispatch, Suspect is Under ~g~Arrest."); 
                }
                GameFiber.Wait(2000);
                Functions.PlayScannerAudio("REPORT_RESPONSE_COPY_02");
                GameFiber.Wait(2000);

                End();
            }
            );
        }
        public override void End()
        {
            Game.LogTrivial("First Callout Finished, Cleaning Up Now.");

            Functions.PlayScannerAudio("WE_ARE_CODE_4");

            if (SuspectBlip.Exists()) SuspectBlip.Delete();

            if (Suspect.Exists()) Suspect.Dismiss(); 

            Game.LogTrivial("First Callout Finished Cleaning Up.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            base.End();
        }
    }
}
