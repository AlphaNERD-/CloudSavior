
using System;
using System.Net;

namespace CloudSavior.Jobs
{
    public class GameFinderJob
    {
        public static void FindGames()
        {
            using (WebClient client = new WebClient())
            {
                string repoUrl = "https://github.com/mtkennerly/ludusavi-manifest";
                string filePath = "data/manifest.yml";
                string downloadUrl = $"{repoUrl}/raw/main/{filePath}";
                string savePath = "C:/Users/AlphaNERD/Desktop/CloudSavior/manifest.yml";

                client.DownloadFile(downloadUrl, savePath);
            }
        }
    }
}