using System.Collections.Generic;

namespace CloudSavior.Objects
{
    public class LudusaviSchema
    {
        public Dictionary<string, GameProperties> AdditionalProperties { get; set; }

        public class GameProperties
        {
            public Dictionary<string, FileProperties> Files { get; set; }
            public object InstallDir { get; set; }
            public Dictionary<string, List<LaunchProperties>> Launch { get; set; }
            public Dictionary<string, RegistryProperties> Registry { get; set; }
            public SteamProperties Steam { get; set; }
            public GogProperties Gog { get; set; }
            public IdProperties Id { get; set; }
            public string Alias { get; set; }
            public CloudProperties Cloud { get; set; }
            public List<NoteProperties> Notes { get; set; }
        }

        public class FileProperties
        {
            public List<string> Tags { get; set; }
            public List<FileConstraint> When { get; set; }
        }

        public class LaunchProperties
        {
            public string Arguments { get; set; }
            public string WorkingDir { get; set; }
            public List<LaunchConstraint> When { get; set; }
        }

        public class RegistryProperties
        {
            public List<string> Tags { get; set; }
            public List<RegistryConstraint> When { get; set; }
        }

        public class SteamProperties
        {
            public int Id { get; set; }
        }

        public class GogProperties
        {
            public int Id { get; set; }
        }

        public class IdProperties
        {
            public string Flatpak { get; set; }
            public List<int> GogExtra { get; set; }
            public string Lutris { get; set; }
            public List<int> SteamExtra { get; set; }
        }

        public class CloudProperties
        {
            public bool Epic { get; set; }
            public bool Gog { get; set; }
            public bool Origin { get; set; }
            public bool Steam { get; set; }
            public bool Uplay { get; set; }
        }

        public class NoteProperties
        {
            public string Message { get; set; }
        }

        public class FileConstraint
        {
            public string Os { get; set; }
            public string Store { get; set; }
        }

        public class LaunchConstraint
        {
            public int Bit { get; set; }
            public string Os { get; set; }
            public string Store { get; set; }
        }

        public class RegistryConstraint
        {
            public string Store { get; set; }
        }
    }
}