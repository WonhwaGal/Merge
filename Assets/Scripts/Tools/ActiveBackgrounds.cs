using System;
using System.Collections.Generic;

namespace Code.SaveLoad
{
    [Serializable]
    public class ActiveBackgrounds
    {
        public List<int> _actives;
        public ActiveBackgrounds() => _actives = new List<int>();
        public List<int> Actives => _actives;
    }
}