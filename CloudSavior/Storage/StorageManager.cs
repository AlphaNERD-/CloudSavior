using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace CloudSavior.Storages
{
    public static class StorageManager
    {
        private static string _storageConfigPathSettingsFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CloudSavior", "storageConfigSeetingsPath.txt");

        private static string _localStorageConfigPath
        {
            get
            {
                return Path.Join(StorageConfigPath, "local.json");
            }
        }
        private static string _remoteStorageConfigPath
        {
            get
            {
                return Path.Join(StorageConfigPath, "remote.json");
            }
        }

        public static string StorageConfigPath { get; set; }
        public static Storage LocalStorage { get; set; }
        public static List<Storage> RemoteStorage { get; set; }

        public static void LoadStorages()
        {
            if (!File.Exists(_storageConfigPathSettingsFile))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_storageConfigPathSettingsFile));
                File.WriteAllText(_storageConfigPathSettingsFile, Path.GetDirectoryName(_storageConfigPathSettingsFile));
            }

            StorageConfigPath = File.ReadAllText(_storageConfigPathSettingsFile);

            LocalStorage = JsonSerializer.Deserialize<Storage>(File.ReadAllText(_localStorageConfigPath));
            RemoteStorage = JsonSerializer.Deserialize<List<Storage>>(File.ReadAllText(_remoteStorageConfigPath));
        }

        public static void SaveStorages()
        {
            File.WriteAllText(_localStorageConfigPath, JsonSerializer.Serialize(LocalStorage));
            File.WriteAllText(_remoteStorageConfigPath, JsonSerializer.Serialize(RemoteStorage));
        }
    }
}