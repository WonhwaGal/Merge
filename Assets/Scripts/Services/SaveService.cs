using System;
using GamePush;
using UnityEngine;

namespace Code.SaveLoad
{
    public sealed class SaveService : IService
    {
        private readonly ProgressHandler _handler;

        public SaveService()
        {
            _handler = new();
            GameEventSystem.Subscribe<ManageDropEvent>(GatherData);
        }

        public ProgressData ProgressData { get; private set; }

        public bool LoadProgress()
        {
            ProgressData = JsonUtility.FromJson<ProgressData>
                (GP_Player.GetString(Constants.DropList)) ?? new(null);
            return ProgressData.SavedDropList != null && ProgressData.SavedDropList.Count > 0;
        }

        public void GatherData(ManageDropEvent @event)
        {
            if (!@event.ReturnToPool)
                _handler.FillData(@event.Drop);
        }

        public void SaveData(float currentScore, bool onlyScore)
        {
            GP_Player.SetScore(GetBestScore(currentScore));
            if(!onlyScore && currentScore != 0 && _handler.Drops.Count > 0)
            {
                GP_Player.Set(Constants.SavedScore, currentScore);
                ProgressData = new ProgressData(_handler.Drops);
                string json = JsonUtility.ToJson(ProgressData);
                GP_Player.Set(Constants.DropList, json);
            }

            GP_Player.Sync();
            ClearData();
        }

        public void ClearData() => _handler.Clear();

        private float GetBestScore(float current)
        {
            var bestScore = GP_Player.GetScore();
            return (current > bestScore) ? current : bestScore;
        }

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<ManageDropEvent>(GatherData);
            GC.SuppressFinalize(this);
        }
    }
}