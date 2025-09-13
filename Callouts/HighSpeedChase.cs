using LSPD_First_Response.Engine;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using RealisticCallouts.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealisticCallouts.callouts
{

    [CalloutInfo("High Speed Chase", CalloutProbability.Medium)]
    public class HighSpeedChase : Callout
    {
        private String[] StolenVehicles = new String[] {"NINFEF2", "BUS", "COACH", "AIRBUS", "BARRACKS", "BARRACKS2", "BALLER", "BALLER2", "BANSHEE", "BJXL", "BENSON", "BOBCATXL",
            "BUCCANEER", "BUFFALO", "BUFFALO2", "BULLDOZER", "BULLET", "BURRITO", "BURRITO2", "BURRITO3", "BURRITO4", "BURRITO5", "CAVALCADE", "CAVALCADE2", "GBURRITO",
            "CAMPER", "CARBONIZZARE", "CHEETAH", "COMET2", "COGCABRIO", "COQUETTE", "GRESLEY", "DUNE2", "HOTKNIFE", "DUBSTA", "DUBSTA2", "DUMP", "DOMINATOR", "EMPEROR", "EMPEROR2",
            "EMPEROR3", "ENTITYXF", "EXEMPLAR", "ELEGY2", "F620", "FELON", "FELON2", "FELTZER2", "FIRETRUK", "FQ2", "FUGITIVE", "FUTO", "GRANGER", "GAUNTLET", "HABANERO",
            "INFERNUS", "INTRUDER", "JACKAL", "JOURNEY", "JB700", "KHAMELION", "LANDSTALKER", "MESA", "MESA2", "MESA3", "MIXER", "MINIVAN", "MIXER2", "MULE", "MULE2", "ORACLE", "ORACLE2",
            "MONROE", "PATRIOT", "PBUS", "PACKER", "PENUMBRA", "PEYOTE", "PHANTOM", "PHOENIX", "PICADOR", "POUNDER", "PRIMO","RANCHERXL", "RANCHERXL2", "RAPIDGT", "RAPIDGT2", "RENTALBUS",
            "RUINER", "RIPLEY", "SABREGT", "SADLER", "SADLER2", "SANDKING", "SANDKING2","SPEEDO", "SPEEDO2", "STINGER", "STOCKADE", "STINGERGT", "SUPERD", "STRATUM", "SULTAN", "AKUMA"};
        
        private Vehicle StolenVehicle;
        private Ped Suspect;
        private Vector3 Spawnpoint;
        private Blip SuspectBlip;
        private LHandle Pursuit;
        private bool PursuitCreated;
        private Vehicle SuspectVhicle;

        public override bool OnBeforeCalloutDisplayed()
        {
            {
                Spawnpoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
                AddMinimumDistanceCheck(300f, Spawnpoint);
                ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
                CalloutMessage = "We Have A High Speed Chase Respond ~w~Code 3";
                CalloutPosition = Spawnpoint;
                Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", Spawnpoint);

                return base.OnBeforeCalloutDisplayed();
            }

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

            if (Settings.ActivateAIBackup)

            
                Functions.RequestBackup(Spawnpoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit);

            } else { Settings.ActivateAIBackup = true; }

            return base.OnCalloutAccepted();
        }
        public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }

            Game.LogTrivial("RealisticCallouts - High Speed Chase Up");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        }
    }
}
