using System;
using System.Collections.Generic;
using CloudSavior.FileSystemAdapter;

namespace CloudSavior.Objects
{
    public class Machine
    {
        public IFileSystemAdapter FileSystemAdapter { get; set; }
        public List<Game> Games { get; set; }
        public List<SearchDirectory> SearchDirectories { get; set; }
    }
}