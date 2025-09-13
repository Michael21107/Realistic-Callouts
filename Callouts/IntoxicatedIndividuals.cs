//using LSPD_First_Response.Engine;
//using LSPD_First_Response.Mod.API;
//using LSPD_First_Response.Mod.Callouts;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;
//using Rage.Native;


//namespace RealisticCallouts.callouts
//{
    //[CalloutInfo("Intoxicated Individual (REMOVED FROM 2.0.0 DUE BUGS AND ISSUSES)", CalloutProbability.High)]
    //internal class IntoxicatedIndividuals : Callout
    //{
        //private string[] PedList = new string[] { "u_m_o_finguru_01", "a_f_y_fitness_01","a_f_m_eastsa_01", "cs_drfriedlander", "cs_denise", "a_f_y_hipster_02", "a_m_y_hipster_03",
        //"a_m_m_fatlatin_01", "s_f_y_hooker_01", "a_f_y_hipster_04", "u_f_y_hotposh_01", "cs_janet", "a_m_y_jetski_01", "player_zero", "a_m_o_tramp_01",
        //"a_m_m_trampbeac_01"," ig_wade"};
        //private Ped Suspect;
        //private Blip SuspectBlip;
        //private Vector3 Spawnpoint;  
        //private float heading;
        //private int counter;
        //private object suspect;
        //private object player;

        //public override bool OnBeforeCalloutDisplayed()
        //{
            //{
                //Spawnpoint = new Vector3(-1573.039f, -1169.825f, 2.402837f);
                //heading = 66.64632f;
                //ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
                //CalloutMessage = "Intoxicated person in public Respond ~y~Code 2";
                //CalloutPosition = Spawnpoint;
                //Functions.PlayScannerAudio("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION");

                //return base.OnBeforeCalloutDisplayed();

            //}
        //}
        //public override bool OnCalloutAccepted()
        //{
            //Suspect = new Ped(PedList[new Random().Next((int)PedList.Length)], Spawnpoint, 0f);
            //Suspect.IsPersistent = true;
            //Suspect.BlockPermanentEvents = true;
            //MAKE SUSPECT DRUNK WITH NATIVES
            //NativeFunction.Natives.SET_​PED_​IS_​DRUNK(Suspect, true);

            //SuspectBlip = Suspect.AttachBlip();
            //SuspectBlip.Color = System.Drawing.Color.DarkGreen;
            //SuspectBlip.IsRouteEnabled = true;

            //counter = 0;

            //return base.OnCalloutAccepted();

        //}
        //public override void Process()
        //{
           // base.Process();

            //if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
            //{
               //Game.DisplayHelp("Press ~y~Y ~w~to talk to the suspect", false);

               // if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
               // {
                 //   counter++;

                  ///  if (counter == 1)
                  //  {
                //        Game.DisplaySubtitle("~r~Player: ~w~Hey stop rigth there for me!");
                //    }
               //     if (counter == 2)
                 //   {
                  //      Game.DisplaySubtitle("~r~Suspect: ~w~What do You want Officer? I dont remember calling the cops!");
                //    }
                //    if (counter == 3)
                //    {
                 //       Game.DisplaySubtitle("~r~Player: ~w~Actually, someone called us saying that you were trying to drinking in public.");
               //     }
                //    if (counter == 4)
              //      {
                //        Game.DisplaySubtitle("~r~Susbect: ~w~What *hic* you got the *hic* Wong Person Officer");
              //      }
               //     if (counter >= 5)
             //       {
             //           Game.DisplaySubtitle("~r~Player: Alright, Hang tight right here for me.");
                 //   }
            //        if (counter >= 6)
                 //   {
                //        Game.DisplaySubtitle("~y~Player: No further dialouge. Do as you see fit.");
                //    }
               // }

           // }
          //  if (Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
          //  {
          //      End();
          //  }
     //   }
       // public override void End()
    //    {
       //     base.End();

       //    if (Suspect.Exists())
        //    {
              //  Suspect.Dismiss();
        //    }
        //    if (SuspectBlip.Exists())
         //   {
       //         SuspectBlip.Delete();
         //   }

        //    Game.LogTrivial("RealisticCallouts - Intoxicated Individuals cleaned up");
      //      Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
       // }
 //   }
//}