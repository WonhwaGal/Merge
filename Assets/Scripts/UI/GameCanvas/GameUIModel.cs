using System;
using Code.MVC;
using UnityEngine;
using GamePush;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameUIModel : IModel, IDisposable
{
    private DropObjectSO _dropData;
    private float _currentScore;
    private int _playerRating;
    private bool _bombActive;
    private int _rewardActivationSpan;
    private AchievementService _achievementService;

    public int MergedRank { get; set; }
    public int PlayerRating => _playerRating;

    public event Action<bool> OnActivateReward;
    public event Action<int> OnGetRating;
    public event Action<string> OnLanguageChanged;

    public void Init(DropObjectSO dropData)
    {
        _dropData = dropData;
        _rewardActivationSpan = GP_Variables.GetInt("RewardActivationSpan");
        GP_Leaderboard.OnFetchPlayerRatingSuccess += OnFetchRating;
        GP_Ads.OnAdsClose += OnRewardClose;
        GP_Ads.OnAdsStart += OnRewardStart;
        GameEventSystem.Subscribe<SaveEvent>(SaveBombStatus);
        _achievementService = ServiceLocator.Container.RegisterAndAssign(new AchievementService());
        RenewRating();
        UpdateTextAsync();
    }

    public void OpenAchievements() => _achievementService.Open();
    public void OpenLeaderBoard() => GP_Leaderboard.Open(withMe: WithMe.first);

    private void UpdateTextAsync() =>
        LocalizationSettings.StringDatabase.GetTableAsync("TextTable").Completed +=
            handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var table = handle.Result;
                    OnLanguageChanged?.Invoke(table.GetEntry("nextUI")?.GetLocalizedString());
                }
            };

    #region Rating
    public void RenewRating() => GP_Leaderboard.FetchPlayerRating();

    private void OnFetchRating(string category, int rating)
    {
        if (rating <= Constants.ShowableRating)
            _playerRating = rating;
        else
            _playerRating = 0;
        OnGetRating?.Invoke(_playerRating);
    }
    #endregion

    #region Score
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
        int firstCheck = (int)_currentScore / _rewardActivationSpan;
        _currentScore = currentScore + _dropData.FindObjectData(MergedRank - 1).MergeRewardPoint;
        int secondCheck = (int)_currentScore / _rewardActivationSpan;

        if (secondCheck > firstCheck && !_bombActive)
            SetBombStatus(true);
        _achievementService.CheckForUnlock(AchievementType.Score, _currentScore);
        return _currentScore;
    }
    #endregion

    #region Rewards
    public void ShowRewardAd() => GP_Ads.ShowRewarded(Constants.BOMB, OnRewardSuccessful);
    private void OnRewardSuccessful(string key)
    {
        if (key != Constants.BOMB)
            return;
        SetBombStatus(false);
        GameEventSystem.Send(new RewardEvent(Constants.BombRank));
    }

    private void OnRewardStart() 
        => GameEventSystem.Send(new SoundEvent(SoundType.BackGround, false));
    private void OnRewardClose(bool arg1)
        => GameEventSystem.Send(new SoundEvent(SoundType.BackGround, true));

    private void SetBombStatus(bool toActivate)
    {
        OnActivateReward?.Invoke(toActivate);
        _bombActive = toActivate;
    }

    private void SaveBombStatus(SaveEvent @event)
    {
        if (!@event.OnlyScore)
        {
            GP_Player.Set(Constants.BombAvailable, _bombActive);
            GP_Player.Sync();
        }
    }
    #endregion

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<SaveEvent>(SaveBombStatus);
        GP_Ads.OnAdsClose -= OnRewardClose;
        GP_Ads.OnAdsStart -= OnRewardStart;
        OnLanguageChanged = null;
        OnActivateReward = null;
        OnGetRating = null;
    }
}