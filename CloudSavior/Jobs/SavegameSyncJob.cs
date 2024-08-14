using System;
using System.Collections.Generic;
using CloudSavior.Storages;
using CloudSavior.Objects;
using System.Linq;

namespace CloudSavior.Jobs
{
    public class SavegameSyncJob
    {
        public static void SyncSavegames()
        {
            List<Storage> storages = new List<Storage>();
            storages.Add(StorageManager.LocalStorage);
            storages.AddRange(StorageManager.RemoteStorage);

            Dictionary<Game, List<Storage>> gameStorages = new Dictionary<Game, List<Storage>>();

            foreach (var storage in storages)
            {
                foreach (var game in storage.Games)
                {
                    if (!gameStorages.ContainsKey(game))
                    {
                        gameStorages.Add(game, new List<Storage>());
                    }

                    gameStorages[game].Add(storage);
                }
            }

            foreach (var game in gameStorages.Keys)
            {
                List<Storage> gameStorage = gameStorages[game];

            }
        }

        public static void SyncDirectory(Dictionary<Storage, FileSystemDirectory> directory)
        {
            Dictionary<string, List<Storage>> fileStorages = new Dictionary<string, List<Storage>>();
            Dictionary<FileSystemDirectory, List<Storage>> directoryStorages = new Dictionary<FileSystemDirectory, List<Storage>>();

            foreach (var storage in directory.Keys)
            {
                FileSystemDirectory fsDir = directory[storage];
                foreach (var file in fsDir.Files)
                {
                    if (!fileStorages.ContainsKey(file))
                    {
                        fileStorages.Add(file, new List<Storage>());
                    }

                    fileStorages[file].Add(storage);
                }

                foreach (var dir in fsDir.Directories)
                {
                    if (!directoryStorages.ContainsKey(dir))
                    {
                        directoryStorages.Add(dir, new List<Storage>());
                    }

                    directoryStorages[dir].Add(storage);
                }
            }

            foreach (var file in fileStorages.Keys)
            {
                List<Storage> fileStorage = fileStorages[file];
                if (fileStorage.Count > 1)
                {
                    Dictionary<Storage, DateTimeOffset> lastWriteTimes = new Dictionary<Storage, DateTimeOffset>();

                    foreach (var storage in fileStorage)
                    {
                        lastWriteTimes.Add(storage, storage.GetLastWriteTime(file));
                    }

                    Storage sourceStorage = lastWriteTimes.Where(x => x.Value == lastWriteTimes.Values.Max()).First().Key;

                    lastWriteTimes.Remove(sourceStorage);

                    foreach (var storage in lastWriteTimes.Keys)
                    {
                        storage.CopyFile(file, sourceStorage, file);
                    }
                }
                else
                {
                    Storage storage = fileStorage.First();
                    storage.CopyFile(file, storage, file);
                }
            }

            foreach (var dir in directoryStorages.Keys)
            {
                List<Storage> dirStorage = directoryStorages[dir];

                if (dirStorage.Count < 2)
                {
                    Storage storage = dirStorage.First();
                    storage.CreateDirectory(dir.Path);
                }
            }
        }
    }
}