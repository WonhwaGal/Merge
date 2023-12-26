using System.Collections.Generic;
using UnityEngine;

namespace Code.SaveLoad
{
    [System.Serializable]
    public class ProgressData
    {
        public List<DropSave> _savedDropList;

        public ProgressData(List<DropSave> dropList) => _savedDropList = dropList;

        public List<DropSave> SavedDropList => _savedDropList;


        [System.Serializable]
        public struct DropSave
        {
            public int Rank;
            public Vector3 Position;

            public DropSave(int rank, Vector3 pos)
            {
                Rank = rank;
                Position = pos;
            }
        }
    }
}