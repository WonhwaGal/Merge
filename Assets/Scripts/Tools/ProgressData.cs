using System.Collections.Generic;
using UnityEngine;

namespace Code.SaveLoad
{
    [System.Serializable]
    public class ProgressData
    {
        public int _bestScore;
        public int _savedScore;
        public List<DropSave> _savedDropList;

        public ProgressData(int bestScore, int currentScore, List<DropSave> dropList)
        {
            _bestScore = bestScore;
            _savedScore = currentScore;
            _savedDropList = dropList;
        }

        public int BestScore { get => _bestScore; set => _bestScore = value; }
        public int SavedScore => _savedScore;
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