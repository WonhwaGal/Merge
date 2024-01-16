using Code.Achievements;

public static class MergeCounter
{
    private const int MinInARow = 3;
    private static int _lastMergingRank;
    private static int _mergesInARow;
    private static int _bombUse;
    private static AchievementService _achievService;

    public static int MergesInARow { get => _mergesInARow; set => _mergesInARow = value; }

    public static void ReceiveMergeInfo(int mergingRank)
    {
        if (_lastMergingRank == mergingRank)
        {
            if (_achievService == null)
                _achievService = ServiceLocator.Container.RequestFor<AchievementService>();

            _achievService.CheckAchievement(AchievType.DoubleMerge, 1);
        }

        _lastMergingRank = mergingRank;
        CheckMergesInARow();
    }

    public static void BombUse()
    {
        _bombUse++;
        _achievService.CheckAchievement(AchievType.BombUse, _bombUse);
    }

    private static void CheckMergesInARow()
    {
        _mergesInARow++;
        if (MergesInARow >= MinInARow)
            _achievService.CheckAchievement(AchievType.MergesInARow, _mergesInARow);
    }
}