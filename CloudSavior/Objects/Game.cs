using System;
using System.Collections.Generic;

namespace CloudSavior.Objects
{
    public class Game
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public int RAWGPlatformID { get; set; }
        public List<string> SaveFiles { get; set; }
        public List<string> ConfigFiles { get; set; }
        public List<string> RegistryKeys { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            Game tempObj = (Game)obj;

            return this.ID == tempObj.ID && this.RAWGPlatformID == tempObj.RAWGPlatformID;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, RAWGPlatformID);
        }
    }
}