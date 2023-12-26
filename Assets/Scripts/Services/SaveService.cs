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
            ProgressData = JsonUtility.FromJson<ProgressData>(GP_Player.GetString("drop_progress")) ?? new(null);
            return ProgressData.SavedDropList != null && ProgressData.SavedDropList.Count > 0;
        }

        public void GatherData(ManageDropEvent @event)
        {
            if (!@event.ReturnToPool)
                _handler.FillData(@event.Drop);
        }

        public void SaveData(float currentScore, bool onlyScore)
        {
            GP_Player.Set("best_score", GetBestScore(currentScore));

            if(!onlyScore)
            {
                GP_Player.SetScore(currentScore);
                ProgressData = new ProgressData(_handler.Drops);
                string json = JsonUtility.ToJson(ProgressData);
                GP_Player.Set("drop_progress", json);
            }

            GP_Player.Sync();
            ClearData();
        }

        public void ClearData() => _handler.Clear();

        private float GetBestScore(float current)
        {
            var bestScore = GP_Player.GetInt("best_score");
            return (current > bestScore) ? current : bestScore;
        }

        public void Dispose()
        {
            GameEventSystem.UnSubscribe<ManageDropEvent>(GatherData);
            GC.SuppressFinalize(this);
        }
    }
}