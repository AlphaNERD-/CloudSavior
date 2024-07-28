using System;

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

                if (gameStorage.Count > 1)
                {

                }
            }
        }
    }
}