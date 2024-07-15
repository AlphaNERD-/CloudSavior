using System;

namespace CloudSavior.Objects
{
    public class FTPSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Anonymous { get; set; }
    }
}