
using Code.SaveLoad;
using UnityEngine;

namespace Code.MVC
{
    public sealed class MenuModel: IModel
    {
        private readonly SaveService _saveService;

        public MenuModel()
        {
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
            GetBestScore();
        }

        public int BestScore { get; private set; }
        public int CurrentScore { get; private set; }

        public void OnSaveData(int currentScore, bool onlyScore)
            => _saveService.SaveData(currentScore, onlyScore);


        public void GetBestScore()
        {
            BestScore = _saveService.ProgressData == null ? 
                0 : _saveService.ProgressData.BestScore;
            CurrentScore = _saveService.ProgressData == null ? 
                0 : _saveService.ProgressData.SavedScore;
        }
    }
}