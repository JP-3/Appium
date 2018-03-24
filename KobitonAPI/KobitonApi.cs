using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace KobitonAPI
{
    public class KobitonApi
    {
        private readonly string send;
        private readonly RestClient client;

        private readonly byte[] byteArray = Encoding.ASCII.GetBytes("");

        public KobitonApi()
        {
            send = "Basic " + Convert.ToBase64String(byteArray);

            client = new RestClient("https://api.kobiton.com/v1/");
        }

        public List<Apps> GetApps()
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: Getting Applications");

            var request = new RestRequest("apps", Method.GET);

            request.AddHeader("Authorization", send);

            var response = client.Execute(request);

            var releases = JArray.Parse("[" + response.Content + "]");

            var kobitonApplications = releases.Children().Children().Children().Children();

            var apps = kobitonApplications.Select(d => d.ToObject<Apps>()).ToList();

            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: found {apps.Count} apps");

            return apps;
        }

        public int PushApp(FileInfo file)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: Pushing application {file.FullName}");

            var appUrl = GenerateUploadAppInformation(file);

            PushApplicationToKobiton(appUrl, file);

            var runAppId = NotifyKobitonOfUpload(appUrl, file);

            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: Awaiting Kobiton server to complete processing app prior to test execution.");

            System.Threading.Thread.Sleep(60000);

            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: Pushed application.  App Id: {runAppId}");

            return runAppId;
        }

        public void DeleteApp(int deleteAppId)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: Deleting App {deleteAppId}");

            var request = new RestRequest($"apps/{deleteAppId}", Method.DELETE);

            request.AddHeader("Authorization", send);

            var response = client.Execute(request);
        }

        private AppURL GenerateUploadAppInformation(FileInfo file)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: GenerateUploadAppInformation {file.FullName}");

            var request = new RestRequest("apps/uploadUrl", Method.POST);

            request.AddHeader("Authorization", send);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            request.AddJsonBody(new
            {
                filename = file.Name,
            });

            try
            {
                var response = client.Execute<AppURL>(request);

                return response.Data;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw new Exception("Exception generating application upload information.", exc);
            }
        }

        private void PushApplicationToKobiton(AppURL appUrl, FileInfo file)
        {
            if (appUrl == null || string.IsNullOrEmpty(appUrl.Url))
            {
                throw new ArgumentNullException(nameof(appUrl));
            }

            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: PushApplicationToKobiton {file.FullName}");

            HttpWebRequest httpRequest = WebRequest.Create(appUrl.Url) as HttpWebRequest;
            httpRequest.Method = "PUT";

            httpRequest.ContentType = "application/octet-stream";
            httpRequest.Headers.Add("x-amz-tagging", "unsaved=true");

            using (Stream dataStream = httpRequest.GetRequestStream())
            {
                byte[] buffer = new byte[8000];
                using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dataStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
        }

        private int NotifyKobitonOfUpload(AppURL appUrl, FileInfo file)
        {
            Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss:fff} KobitonAPI: NotifyKobitonOfUpload {file.FullName}");

            var request = new RestRequest("apps", Method.POST);

            request.AddHeader("Authorization", send);
            request.AddHeader("content-type", "application/json");

            request.AddJsonBody(new
            {
                filename = file.Name,
                appPath = appUrl.AppPath,
            });

            var response = client.Execute<AppIdentification>(request);

            return response.Data.AppId;
        }
    }
}
