using System;
using UnityEngine;

namespace Code.SaveLoad
{
    public sealed class SaveService : IService
    {
        private readonly SaveHandler _handler;

        public SaveService()
        {
            _handler = new();
            GameEventSystem.Subscribe<ManageDropEvent>(GatherData);
        }

        public ProgressData ProgressData { get; private set; }

        public ProgressData LoadProgress() => ProgressData = LoadHandler.Load() ?? new(0, 0, null);

        public void GatherData(ManageDropEvent @event)
        {
            if (!@event.ReturnToPool)
                _handler.FillData(@event.Drop);
        }

        public void SaveData(int currentScore, bool onlyScore)
        {
            ProgressData progressData;
            if (onlyScore)
            {
                ProgressData.BestScore = GetBestScore(currentScore);
                progressData = ProgressData;
            }
            else
            {
                progressData = new ProgressData(GetBestScore(currentScore), currentScore, _handler.Drops);
            }
            LoadHandler.Save(progressData);
            //Debug.Log(Application.persistentDataPath + "/DataSaver.json");
            ClearData();
        }

        public void ClearData() => _handler.Clear();
        private int GetBestScore(int current)
            => (current > ProgressData.BestScore) ? current : ProgressData.BestScore;

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<ManageDropEvent>(GatherData);
            GC.SuppressFinalize(this);
        }
    }
}