/*
 * Author: Zolee
 * Plugin Name: Police Whistle
 * Settings.cs
 */

using System.IO;
using System.Reflection;
using Rage;
using System.Windows.Forms;

namespace PoliceWhistle.Classes
{
    internal static class Settings
    {
        private static readonly InitializationFile IniFile =
            new InitializationFile($"plugins/LSPDFR/{Assembly.GetExecutingAssembly().GetName().Name}.ini");

        /// <summary>
        /// Loads the values from the .ini file, if there are none it gives them default values
        /// </summary> 
        internal static void LoadSettings()
        {
            Globals.General.WhistleProbability = IniFile.ReadInt32("General", "WhistleProbability", 30);
            if (Globals.General.WhistleProbability <= 1 || 100 <= Globals.General.WhistleProbability)
            {
                Logging.Log("Incorrect value given for WhistleProbability defaulting to 30!");
                Globals.General.WhistleProbability = 30;
            }
            Globals.Controls.WhistleKey = IniFile.ReadEnum("Keys", "WhistleKey", Keys.X);
            Globals.Controls.WhistleModifierKey = IniFile.ReadEnum("Keys", "WhistleModifierKey", Keys.LShiftKey);
            Globals.Debug.DebugMode = IniFile.ReadBoolean("Debug", "DebugMode", false);
            Globals.Radar.IsTrafficStopped = false;
            
            if (Globals.Debug.DebugMode) Logging.DebugLog($"Settings loaded, values: WhistleProbability = {Globals.General.WhistleProbability}, WhistleKey = {Globals.Controls.WhistleKey}, WhistleModifierKey = {Globals.Controls.WhistleModifierKey}");
        }

        /// <summary>
        /// Checks if the given file exists, if not, it calls the Create() function
        /// </summary> 
        internal static void DoesFileExists()
        {
            if (File.Exists($"plugins/LSPDFR/{Assembly.GetExecutingAssembly().GetName().Name}.ini")) return;
            Create();
            if (Globals.Debug.DebugMode) Logging.DebugLog("Settings file not found, creating it");
        }

        /// <summary>
        /// Creates the .ini file if it doesn't exists
        /// </summary> 
        private static void Create()
        {
            using (var ini = System.IO.File.AppendText($"plugins/LSPDFR/{Assembly.GetExecutingAssembly().GetName().Name}.ini"))
            {
                ini.WriteLine(
                    $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} by Zolee\n" +
                    "[General]\n" +
                    "# The probability that a ped stops when you whistle (in percent), ranges from 1 to 100 (default: 30)\n" +
                    "WhistleProbability = 30\n\n" +
                    "[Keys]\n" +
                    "# Valid keys can be found here: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8\n" +
                    "# The key to use the \"Car stopping whistle\" with (Default: X)\n" +
                    "WhistleKey = X\n\n" +
                    "# The modifier key to press in addition to the WhistleKey to use the \"Ped stopping whistle\" with (Default: LShiftKey)\n" +
                    "WhistleModifierKey = LShiftKey\n\n" +
                    "[Debug]\n" +
                    "# Enables debug mode - which logs every action, should only be set to true if you know what you are doing (Default: false)!\n" +
                    "DebugMode = false");

                if (Globals.Debug.DebugMode) Logging.DebugLog("Settings file created");
            }
        }
    }
}