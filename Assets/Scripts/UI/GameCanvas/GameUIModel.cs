using System;
using Code.MVC;
using UnityEngine;
using GamePush;

public class GameUIModel : IModel, IDisposable
{
    private DropObjectSO _dropData;
    private float _currentScore;
    private int _playerRating;
    private bool _bombActive;

    public void AssignSources(DropObjectSO dropData)
    {
        _dropData = dropData;
        GP_Leaderboard.OnFetchPlayerRatingSuccess += OnFetchRating;
        GameEventSystem.Subscribe<SaveEvent>(SaveBombStatus);
        RenewRating();
    }

    public int MergedRank { get; set; }
    public int PlayerRating => _playerRating;

    public event Action<bool> OnActivateReward;
    public event Action<int> OnGetRating;

    public void RenewRating() => GP_Leaderboard.FetchPlayerRating();

    public Sprite GetNextRank()
    {
        var nextRank = DropQueueHandler.NextDrop;
        return _dropData.FindObjectData(nextRank).DropSprite;
    }

    public float SetScore()
    {
        OnActivateReward?.Invoke(GP_Player.GetBool(Constants.BombAvailable));
        return GP_Player.GetInt(Constants.SavedScore);
    }

    public float GetAddPoints(float currentScore)
    {
        int firstCheck = (int)_currentScore / Constants.RewardActivationSpan;
        _currentScore = currentScore + _dropData.FindObjectData(MergedRank - 1).MergeRewardPoint;
        int secondCheck = (int)_currentScore / Constants.RewardActivationSpan;

        if (secondCheck > firstCheck && !_bombActive)
            SetBombStatus(true);
        return _currentScore;
    }

    public void ShowRewardAd() 
        => GP_Ads.ShowRewarded(Constants.BOMB, OnRewardSuccessful);

    public void OpenLeaderBoard() => GP_Leaderboard.Open(withMe: WithMe.first);

    private void OnRewardSuccessful(string key)
    {
        if (key != Constants.BOMB)
            return;
        SetBombStatus(false);
        GameEventSystem.Send(new RewardEvent(Constants.BombRank));
    }

    private void OnFetchRating(string category, int rating)
    {
        if (rating <= Constants.ShowableRating)
            _playerRating = rating;
        else
            _playerRating = 0;
        OnGetRating?.Invoke(_playerRating);
    }

    private void SetBombStatus(bool toActivate)
    {
        OnActivateReward?.Invoke(toActivate);
        _bombActive = toActivate;
    }

    private void SaveBombStatus(SaveEvent @event)
    {
        if(!@event.OnlyScore)
            GP_Player.Set(Constants.BombAvailable, _bombActive);
    }

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<SaveEvent>(SaveBombStatus);
        OnActivateReward = null;
        OnGetRating = null;
    }
}