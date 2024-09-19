
using System;
using System.Net;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using CloudSavior.Storages;
using CloudSavior.Objects;

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



            var deserializer = new DeserializerBuilder().Build();
            List<LudusaviSchema> manifest = deserializer.Deserialize<List<LudusaviSchema>>(System.IO.File.ReadAllText("C:/Users/AlphaNERD/Desktop/CloudSavior/manifest.yml"));

            List<Storage> storages = new List<Storage>();

            storages.Add(StorageManager.LocalStorage);

            storages.AddRange(StorageManager.RemoteStorage);

            

            foreach (Storage storage in storages)
            {
                
            }
        }
    }
}