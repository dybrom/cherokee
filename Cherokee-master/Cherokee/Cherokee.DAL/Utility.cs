using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Cherokee.DAL
{
    public static class Utility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Log(string Message, string Level = "ERROR", Exception ex = null)
        {
            if (Level == "INFO") log.Info(Message); else log.Error(Message);
        }

        public static string ToFriendlyString(this Enum value)
        {
            var text = value.ToString();
            var regex = new Regex(@"[\p{Lu}^\p{Ll}]\p{Ll}*");
            var split = from Match match in regex.Matches(text) select match.Value.InitialCapital();
            return string.Join(" ", split);
        }
        // Logger.Log("sadasda", "INFO");

        public static string InitialCapital(this string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return word;
            return word.First().ToString().ToUpper() + word.Substring(1);
        }

        public static bool FileExists(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            return File.Exists(fileName);
        }
    }
}