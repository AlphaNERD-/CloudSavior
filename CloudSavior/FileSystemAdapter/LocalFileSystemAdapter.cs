using System;
using System.IO;
using System.Linq;
using CloudSavior.FileSystemAdapter;

namespace CloudSavior.FileSystemAdapter
{
    public class LocalFileSystemAdapter : IFileSystemAdapter
    {
        public override void Connect()
        {
            // Da es sich um das lokale Dateisystem handelt, ist keine explizite Verbindung notwendig.
        }

        public override void Disconnect()
        {
            // Da es sich um das lokale Dateisystem handelt, ist keine explizite Trennung notwendig.
        }

        public override FileSystemDirectory GetDirectory(string path, bool recursive)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directory = new FileSystemDirectory(directoryInfo.FullName)
            {
                Files = directoryInfo.GetFiles().Select(f => f.FullName).ToList()
            };

            foreach (var dir in directoryInfo.GetDirectories())
            {
                if (recursive)
                {
                    directory.Directories.Add(GetDirectory(dir.FullName, true));
                }
                else
                {
                    directory.Directories.Add(new FileSystemDirectory(dir.FullName));
                }
            }

            return directory;
        }

        public override DateTimeOffset GetLastWriteTime(string path)
        {
            return new DateTimeOffset(File.GetLastWriteTime(path));
        }

        protected override byte[] LoadFile(string path)
        {
            return File.ReadAllBytes(path);
        }

        protected override void SaveFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
    }
}