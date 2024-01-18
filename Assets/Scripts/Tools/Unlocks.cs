using System;
using System.Collections.Generic;

namespace Code.SaveLoad
{
    [Serializable]
    public class Unlocks
    {
        public List<int> _unlocks;
        public Unlocks() => _unlocks = new List<int>();
        public List<int> Unlocked => _unlocks;
    }
}