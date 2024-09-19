using System;
using System.Collections.Generic;
using CloudSavior.Storages;
using CloudSavior.Objects;
using System.Linq;
using System.IO;

namespace CloudSavior.Jobs
{
    public class SavegameSyncJob
    {
        private enum SyncDirectoryType
        {
            Savegame,
            Config
        }

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

                Dictionary<Storage, FileSystemDirectory> directories = new Dictionary<Storage, FileSystemDirectory>();

                foreach (var dir in GetSyncDirectories(gameStorage, game, SyncDirectoryType.Savegame, true))
                {
                    directories.Add(dir.Key, dir.Value);
                }

                foreach (var dir in GetSyncDirectories(gameStorage, game, SyncDirectoryType.Config, true))
                {
                    directories.Add(dir.Key, dir.Value);
                }

                SyncDirectory(directories);
            }
        }

        private static Dictionary<Storage, FileSystemDirectory> GetSyncDirectories(List<Storage> gameStorage, Game game, SyncDirectoryType type, bool create)
        {
            Dictionary<Storage, FileSystemDirectory> directories = new Dictionary<Storage, FileSystemDirectory>();

            List<string> directoryList = new List<string>();

            switch (type)
            {
                case SyncDirectoryType.Savegame:
                    directoryList = game.SaveFiles;
                    break;
                case SyncDirectoryType.Config:
                    directoryList = game.ConfigFiles;
                    break;
            }

            foreach (string path in directoryList)
            {
                List<string> pathElements = path.Split('/').ToList();

                string relativePath = "";
                bool pathDetermined = false;

                while (!pathDetermined)
                {
                    relativePath = pathElements.Last();

                    pathElements.RemoveAt(pathElements.Count - 1);

                    if (directoryList.Find(x => x.Contains(relativePath)).Count() > 1)
                    {
                        relativePath = path + "/" + relativePath;
                    }
                    else
                    {
                        pathDetermined = true;
                    }
                }

                foreach (var storage in gameStorage)
                {
                    directories.Add(storage, storage.GetDirectory(storage.Games.Find(x => x == game).SaveFiles.Find(x => x.Contains(relativePath)), false));
                }
            }

            return directories;
        }

        private static void SyncDirectory(Dictionary<Storage, FileSystemDirectory> directory)
        {
            Dictionary<string, List<Storage>> fileStorages = new Dictionary<string, List<Storage>>();
            Dictionary<FileSystemDirectory, List<Storage>> directoryStorages = new Dictionary<FileSystemDirectory, List<Storage>>();

            List<Storage> allStorages = fileStorages.Keys.SelectMany(x => fileStorages[x]).Distinct().ToList();

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

                Dictionary<Storage, DateTimeOffset> lastWriteTimes = new Dictionary<Storage, DateTimeOffset>();

                foreach (var storage in fileStorage)
                {
                    lastWriteTimes.Add(storage, storage.GetLastWriteTime(file));
                }

                Storage sourceStorage = lastWriteTimes.Where(x => x.Value == lastWriteTimes.Values.Max()).First().Key;

                lastWriteTimes.Remove(sourceStorage);

                foreach (var storage in lastWriteTimes.Keys)
                {
                    sourceStorage.CopyFile(file, storage, file);
                }

                List<Storage> missingStorages = allStorages.Where(x => !fileStorages[file].Contains(x)).ToList();

                foreach (var storage in missingStorages)
                {
                    sourceStorage.CopyFile(file, storage, file);
                }
            }

            foreach (var dir in directoryStorages.Keys)
            {
                List<Storage> missingStorages = allStorages.Where(x => !directoryStorages[dir].Contains(x)).ToList();

                foreach (var storage in missingStorages)
                {
                    storage.CreateDirectory(dir.Path);
                    directoryStorages[dir].Add(storage);
                }

                Dictionary<Storage, FileSystemDirectory> subDirectories = new Dictionary<Storage, FileSystemDirectory>();

                foreach (var storage in directoryStorages[dir])
                {
                    subDirectories.Add(storage, storage.GetDirectory(dir.Path, false));
                }

                SyncDirectory(subDirectories);
            }
        }
    }
}