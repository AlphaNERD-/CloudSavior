using System;
using System.IO;
using System.Text.Json;

namespace CloudSavior
{
    public class StorageManager
    {
        private string _storageConfigPathSettingsFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolders.AppData), "CloudSavior", "storageConfigSeetingsPath.txt");

        private string _localStorageConfigPath
        {
            get
            {
                return Path.Join(StorageConfigPath, "local.json");
            }
        }
        private string _remoteStorageConfigPath
        {
            get
            {
                return Path.Join(StorageConfigPath, "remote.json");
            }
        }

        private JsonSerializer serializer = new JsonSerializer();

        public string StorageConfigPath { get; set; }
        public Storage LocalStorage { get; set; }
        public List<Storage> RemoteStorage { get; set; }

        public string LoadStorages()
        {
            if (!File.Exists(_storageConfigPathSettingsFile))
            {
                Directory.CreateDirectory(File.GetFolderPath(_storageConfigPathSettingsFile));
                File.WriteAllText(_storageConfigPathSettingsFile, File.GetFolderPath(_storageConfigPathSettingsFile));
            }

            StorageConfigPath = File.ReadAllText(_storageConfigPathSettingsFile);

            LocalStorage = serializer.Deserialize<Storage>(File.ReadAllText(_localStorageConfigPath));
            RemoteStorage = serializer.Deserialize<List<Storage>>(File.ReadAllText(_remoteStorageConfigPath));
        }

        public string SaveStorages()
        {
            File.WriteAllText(_localStorageConfigPath, serializer.Serialize(LocalStorage));
            File.WriteAllText(_remoteStorageConfigPath, serializer.Serialize(RemoteStorage));
        }
    }
}