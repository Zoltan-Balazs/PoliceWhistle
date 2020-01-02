/*
 * Author: Zolee
 * Plugin Name: Police Whistle
 * Logging.cs
 */

using Rage;
using System.Reflection;

namespace PoliceWhistle.Classes
{
    internal static class Logging
    {
        /// <summary>
        /// Logging information to the user
        /// </summary>
        /// <param name="loggingInformation"></param>
        internal static void Log(string loggingInformation)
        {
            var log = $"{Assembly.GetExecutingAssembly().GetName().Name}: {loggingInformation}";

            Game.LogTrivial(log);
        }
        
        /// <summary>
        /// Logging debug information to the user, should only happen if DebugMode is set to true
        /// </summary>
        /// <param name="debugMessage"></param>
        internal static void DebugLog(string debugMessage)
        {
            var log = $"{Assembly.GetExecutingAssembly().GetName().Name} - DEBUG: {debugMessage}";

            Game.LogTrivial(log);
        }
    }
}