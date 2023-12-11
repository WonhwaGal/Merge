using System.Collections.Generic;
using Code.DropLogic;
using static Code.SaveLoad.ProgressData;

namespace Code.SaveLoad
{
    public sealed class SaveService : IService
    {
        private readonly List<DropSave> _drops = new();
        private ProgressData _progressData;
        private int _bestScore = 0;

        public ProgressData ProgressData => _progressData;

        public ProgressData LoadProgress() => _progressData = LoadHandler.Load();

        public void GatherData(DropObject drop) 
            => _drops.Add(new DropSave(drop.Rank, drop.transform.position));

        public void SaveData(int currentScore, bool onlyScore)
        {
            ProgressData progressData;
            if (onlyScore)
            {
                if (ProgressData == null)
                    _progressData = new(0, 0, null);
                _progressData.BestScore = GetBestScore(currentScore);
                progressData = _progressData;
            }
            else 
            {
                progressData = new ProgressData(GetBestScore(currentScore), currentScore, _drops);
            }
            LoadHandler.Save(progressData);
            //Debug.Log(Application.persistentDataPath + "/DataSaver.json");
            _drops.Clear();
        }

        private int GetBestScore(int current)
        {
            _bestScore = ProgressData.BestScore;
            if(current > _bestScore)
                _bestScore = current;
            return _bestScore;
        }
    }
}