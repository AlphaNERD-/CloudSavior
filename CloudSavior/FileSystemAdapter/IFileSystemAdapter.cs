using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        protected virtual void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public virtual bool CheckTimeSynchronicity(IFileSystemAdapter adapter)
        {
            DeleteFile("test.txt");
            adapter.DeleteFile("test.txt");

            Task taskLocal = Task.Run(() => SaveFile("test.txt", MakeTestFile()));
            Task taskRemote = Task.Run(() => adapter.SaveFile("test.txt", MakeTestFile()));

            Task.WaitAll(taskLocal, taskRemote);

            DateTimeOffset localTime = GetLastWriteTime("test.txt");
            DateTimeOffset remoteTime = adapter.GetLastWriteTime("test.txt");
            return localTime == remoteTime;
        }

        private byte[] MakeTestFile()
        {
            string text = "This is a test file.";
            byte[] fileData = Encoding.UTF8.GetBytes(text);
            return fileData;
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