using System;
using System.Collections.Generic;
using DynamicData.Tests;

namespace CloudSavior.FileSystemAdapter
{
    public class IFileSystemAdapter
    {
        public virtual void Connect()
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual FileSystemDirectory GetDirectory(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public virtual DateTimeOffset GetLastWriteTime(string path)
        {
            throw new NotImplementedException();
        }

        public void CopyFile(string sourcePath, IFileSystemAdapter adapter, string destinationPath)
        {
            byte[] file = LoadFile(sourcePath);
            adapter.SaveFile(destinationPath, file);
        }

        protected virtual byte[] LoadFile(string path)
        {
            throw new NotImplementedException();
        }

        protected virtual void SaveFile(string path, byte[] data)
        {
            throw new NotImplementedException();
        }
    }

    public class FileSystemDirectory
    {
        public FileSystemDirectory(string path)
        {
            Path = path;
        }
        
        public string Path { get; set; }
        public List<FileSystemDirectory> Directories { get; set; } = new List<FileSystemDirectory>();
        public List<string> Files { get; set; } = new List<string>();
    }
}