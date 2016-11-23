using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ReleaseNotifier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month - 1;
            var year = DateTime.Now.Year;

            string en = $"https://forum-en.guildwars2.com/forum/info/updates/Game-Update-Notes-{MonthNames.EnglishMonths[month]}-{day}-{year}";
            string fr = $"https://forum-fr.guildwars2.com/forum/info/updates/Mise-jour-du-jeu-{day}-{MonthNames.FrenchMonths[month]}-{year}";
            string de = $"https://forum-de.guildwars2.com/forum/info/updates/Release-Notes-zum-Spiel-{day}-{MonthNames.GermanMonths[month]}-{year}";
            string es = $"https://forum-es.guildwars2.com/forum/info/updates/Notas-de-actualizaci-n-del-juego-{day}-{MonthNames.SpanishMonths[month]}-{year}";

            Parallel.Invoke(
                () => Connect(en),
                () => Connect(fr),
                () => Connect(de),
                () => Connect(es));
        }

        private static void Connect(string url)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] Checking: {url}");
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "HEAD";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response.ResponseUri.ToString() == url)
            {
                response.Close();
                Process.Start(url);
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now.ToString()}] Response does not match. ({response.ResponseUri})");
                response.Close();
                Thread.Sleep(10000);
                Connect(url);
            }
        }
    }
}
