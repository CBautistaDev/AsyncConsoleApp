using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AsyncConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var watch = Stopwatch.StartNew();
            await RunDownloadParrallelASync();
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Total execution time: {elapsedMs}");

            Console.ReadLine();
        }


        private static List<string> PrepData()
        {
            List<string> output = new List<string>();


            output.Add("https://www.google.com");
            output.Add("https://www.experian.com/");
            return output;
        }


        private static  WebsiteDataModel DownloadWebsite(string websiteURL)
        {
            //private static readonly HttpClient client = new HttpClient();

            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }

        private static void ReportWebsiteInfo(WebsiteDataModel data)
        {

            Console.WriteLine($"{data.WebsiteUrl} downloaded: {data.WebsiteData.Length} characters longs");

        }

        private static async Task RunDownloadASync()
        {

            List<string> websites = PrepData();
            foreach (string site in websites)
            {
                WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));
                ReportWebsiteInfo(results);
            }
        }

        public static async Task RunDownloadParrallelASync()
        {

            List<string> websites = PrepData();

            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            foreach (string site in websites)
            {

                tasks.Add(Task.Run(() => DownloadWebsite(site)));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var item in results)
            {
                ReportWebsiteInfo(item);
            }

        }

    }
}
