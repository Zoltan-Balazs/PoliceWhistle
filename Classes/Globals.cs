/*
 * Author: Zolee
 * Plugin Name: Police Whistle
 * Globals.cs
 */

using System.Windows.Forms;
using Rage;

namespace PoliceWhistle.Classes
{
    public static class Globals
    {
        internal static class General
        {
            public static int WhistleProbability { get; set; }
        }
        internal static class Debug
        {
            public static bool DebugMode { get; set; }
        }

        internal static class Controls
        {
            public static Keys WhistleKey { get; set; }
            public static Keys WhistleModifierKey { get; set; }
        }

        internal static class Radar
        {
            public static bool IsTrafficStopped { get; set; }
            public static Blip TrafficBlip { get; set; }
            public static uint StopZone { get; set; }
        }
    }
}