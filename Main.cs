/*
 * Author: Zolee
 * Plugin Name: Police Whistle
 * Main.cs
 *
 * Special thanks to:
 * - HazyTube, for his open-source plugins
 * - Sebo, for continuously supporting me & helping me out whenever I was stuck 
 * - You, for trying out my plugin/decompiling the DLL to look into its source code
 */

using Rage;
using System;
using LSPD_First_Response.Mod.API;
using PoliceWhistle.Classes;
using System.Media;
using System.Reflection;
using System.Drawing;
using Rage.Native;

[assembly:
    Rage.Attributes.Plugin("PoliceWhistle", Description = "Flavor mod for a more realistic officer experience.",
        Author = "Zolee", PrefersSingleInstance = true)]

namespace PoliceWhistle
{
    public class Main : Plugin
    {
        /// <summary>
        /// Overloaded function of LSPDFR, Initializes the plugin once LSPDFR is loaded
        /// </summary> 
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += DutyStateChange;

            Logging.Log($"v{Assembly.GetExecutingAssembly().GetName().Version.ToString()} has been initialized.");
        }


        /// <summary>
        /// Deals with the main logic of the plugin, depending on whether the user is onDuty or not
        /// </summary>
        /// <param name="OnDuty"></param>
        private void DutyStateChange(bool OnDuty)
        {
            if (OnDuty)
            {
                Logging.Log("Starting plugin");
                Notification.StartUpNotification();

                var player = Game.LocalPlayer.Character;

                Settings.DoesFileExists();
                Settings.LoadSettings();
                
                try
                {
                    GameFiber.StartNew(delegate
                    {
                        while (true)
                        {
                            GameFiber.Yield();
                            try
                            {    
                                if (player.IsAlive && player.IsOnFoot && Game.IsKeyDown(Globals.Controls.WhistleKey) &&
                                    Game.IsKeyDownRightNow(Globals.Controls.WhistleModifierKey))
                                {
                                    if (Globals.Debug.DebugMode) Logging.DebugLog("WhistleKey & WhistleModifierKey pressed");

                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Whistle", "DLC_TG_Running_Back_Sounds", 0);
                                    
                                    var pedsInArea = player.GetNearbyPeds(new Random().Next(1, 16));

                                    if (Globals.Debug.DebugMode) Logging.DebugLog($"{pedsInArea.Length} nearby peds chosen");

                                    foreach (var ped in pedsInArea)
                                    {
                                        if (!ped.IsOnFoot || Functions.IsPedACop(ped) || !ped.IsHuman) continue;

                                        if (new Random().Next(1, 101) < 30)
                                        {
                                            ped.Tasks.StandStill(-1);
                                            
                                            if (Functions.IsPedInPursuit(ped))
                                            {
                                                Functions.SetPursuitDisableAIForPed(ped, true);

                                                NativeFunction.Natives.SET_PED_DROPS_WEAPON(ped);
                                                ped.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                                            }
                                        }

                                        if (Globals.Debug.DebugMode) Logging.DebugLog("Tasks assigned to appropriate peds");
                                    }

                                    if (Globals.Debug.DebugMode) Logging.DebugLog("Chosen peds have been cycled through");
                                    GameFiber.Sleep(1500);
                                }
                                else if (player.IsAlive && player.IsOnFoot && Game.IsKeyDown(Globals.Controls.WhistleKey) && !Game.IsKeyDownRightNow(Globals.Controls.WhistleModifierKey))
                                {
                                    if (Globals.Debug.DebugMode)
                                        Logging.DebugLog("WhistleKey pressed");

                                    if (!Globals.Radar.IsTrafficStopped)
                                    {
                                        Globals.Radar.IsTrafficStopped = true;
                                        if(!player.IsAiming)
                                            player.Tasks.PlayAnimation(new AnimationDictionary("rcmnigel1c"), "hailing_whistle_waive_a", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
                                        else
                                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Whistle", "DLC_TG_Running_Back_Sounds", 0);
                                        Globals.Radar.TrafficBlip = new Blip(player.Position, 50f)
                                        {
                                            Alpha = 0.3f,
                                            Color = Color.FromArgb(3, 182, 252)
                                        };
                                        Globals.Radar.StopZone = World.AddSpeedZone(player.Position, 50f, 0f);

                                        if (Globals.Debug.DebugMode) Logging.DebugLog("Radar Blip & StopZone created");

                                        GameFiber.Sleep(1500);
                                    }
                                    else
                                    {
                                        if(!player.IsAiming)
                                            player.Tasks.PlayAnimation(new AnimationDictionary("rcmnigel1c"), "hailing_whistle_waive_a", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
                                        else
                                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Whistle", "DLC_TG_Running_Back_Sounds", 0);
                                        Globals.Radar.IsTrafficStopped = false;
                                        Globals.Radar.TrafficBlip.Delete();
                                        World.RemoveSpeedZone(Globals.Radar.StopZone);

                                        if (Globals.Debug.DebugMode) Logging.DebugLog("Radar Blip & StopZone removed");
                                        
                                        GameFiber.Sleep(1500);
                                    }
                                }
                            }
                            catch
                            {
                                //ignored
                            }
                        }
                    });
                }
                catch
                {
                    if (Globals.Debug.DebugMode) Logging.DebugLog("Exception thrown during onDuty - SERIOUS ISSUE!");
                }
            }
            else
            {
                Finally();
                if (Globals.Debug.DebugMode) Logging.DebugLog("Finally() called");
            }
        }


        /// <summary>
        /// Overloaded function of LSPDFR, deals with logging & notifying when LSPDFR crashes/is unloaded
        /// </summary> 
        public override void Finally()
        {
            if (Globals.Radar.IsTrafficStopped)
            {
                Globals.Radar.IsTrafficStopped = false;
                Globals.Radar.TrafficBlip.Delete();
                World.RemoveSpeedZone(Globals.Radar.StopZone);
            }

            Notification.FinallyNotification();

            if (Globals.Debug.DebugMode) Logging.DebugLog("Finally() executed, radar cleaned up");
        }
    }
}