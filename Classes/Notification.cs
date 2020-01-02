/*
 * Author: Zolee
 * Plugin Name: Police Whistle
 * Notifications.cs
 */

using Rage;
using System.Reflection;

namespace PoliceWhistle.Classes
{
    internal static class Notification
    {
        /// <summary>
        /// Logging &amp; Notifying the user when the plugin is loaded
        /// </summary> 
        internal static void StartUpNotification()
        {
            Game.DisplayNotification(
                $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} ~b~by Zolee ~g~has been loaded.");
            Logging.Log("Plugin successfully loaded.");
        }

        /// <summary>
        /// Logging &amp; Notifying the user when the plugin is unloaded
        /// </summary>
        internal static void FinallyNotification()
        {
            Game.DisplayNotification(
                $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} ~b~by Zolee ~r~has been unloaded.");
            Logging.Log("Plugin has been successfully unloaded.");
        }
    }
}