using System;
using Code.MVC;
using UnityEngine;
using GamePush;

public class GameUIModel : IModel
{
    private DropObjectSO _dropData;
    private const float RewardActivationPoints = 500;
    private float _rewardGrantedPoints;
    private float _currentScore;

    public void AssignDataSource(DropObjectSO dropData) => _dropData = dropData;

    public int MergedRank { get; set; }

    public event Action<bool> OnActivateReward;

    public Sprite GetNextRank()
    {
        var nextRank = DropQueueHandler.NextDrop;
        return _dropData.FindObjectData(nextRank).DropSprite;
    }

    public float SetScore() => GP_Player.GetInt(Constants.SavedScore);

    public float GetAddPoints(float currentScore)
    {
        _currentScore = currentScore + _dropData.FindObjectData(MergedRank - 1).MergeRewardPoint;
        var gotEnough = _currentScore - _rewardGrantedPoints >= RewardActivationPoints;
        if (gotEnough)
            OnActivateReward?.Invoke(true);
        return _currentScore;
    }

    public void ShowRewardAd()
    {
        GP_Ads.ShowRewarded(Constants.BOMB, OnRewardSuccessful);
        OnActivateReward?.Invoke(false);
        _rewardGrantedPoints = _currentScore;
    }

    private void OnRewardSuccessful(string key)
    {
        if (key == Constants.BOMB)
            GameEventSystem.Send(new RewardEvent(Constants.BombRank));
    }

    public void OpenLeaderBoard()
    {
        GP_Leaderboard.Open(withMe: WithMe.first);
    }
}