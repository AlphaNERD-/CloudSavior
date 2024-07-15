using System;
using System.Collections.Generic;

namespace CloudSavior.Objects
{
    public class Game
    {
        public string Name { get; set; }
        public int RAWGID { get; set; }
        public int RAWGPlatformID { get; set; }
        public List<string> SaveFiles { get; set; }
        public List<string> ConfigFiles { get; set; }
        public List<string> RegistryKeys { get; set; }
        public List<string> Quicksaves { get; set; }

    }
}