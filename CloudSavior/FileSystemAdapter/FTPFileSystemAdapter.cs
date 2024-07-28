using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudSavior.FileSystemAdapter;

namespace CloudSavior.FileSystemAdapter
{
    public class FtpFileSystemAdapter : IFileSystemAdapter
    {
        private FtpClient client;

        public FtpFileSystemAdapter(string host, string username, string password, string rootpath)
        {
            client = new FtpClient(host, username, password);
        }

        public override void Connect()
        {
            client.Connect();
        }

        public override void Disconnect()
        {
            client.Disconnect();
        }

        public override FileSystemDirectory GetDirectory(string path, bool recursive)
        {
            var items = client.GetListing(path, recursive ? FtpListOption.AllFiles | FtpListOption.Recursive : FtpListOption.AllFiles);
            var directory = new FileSystemDirectory(path) 
            { 
                Files = items.Where(i => i.Type == FtpObjectType.File).Select(i => i.FullName).ToList() 
            };

            foreach (var item in items.Where(i => i.Type == FtpObjectType.Directory))
            {
                if (recursive)
                {
                    directory.Directories.Add(GetDirectory(item.FullName, true));
                }
                else
                {
                    directory.Directories.Add(new FileSystemDirectory(item.FullName));
                }
                break;
            }

            return directory;
        }

        public override DateTimeOffset GetLastWriteTime(string path)
        {
            return client.GetModifiedTime(path);
        }

        protected override byte[] LoadFile(string path)
        {
            using (var ms = new MemoryStream())
            {
                client.DownloadStream(ms, path);
                return ms.ToArray();
            }
        }

        protected override void SaveFile(string path, byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                client.UploadStream(ms, path);
            }
        }

        protected override void DeleteFile(string path)
        {
            client.DeleteFile(path);
        }
    }
}