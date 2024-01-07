using GamePush;

public class AchievementService : IService
{
    public void Open() => GP_Achievements.Open();

    public void CheckForUnlock(AchievementType type, float data)
    {
        if (type == AchievementType.Score && data >= 200)
            GP_Achievements.Unlock("score_1");
    }
}

public enum AchievementType
{
    Score = 0,
    MergeByRank = 1,
    MergeByGame = 2,
    DoubleMerge = 5,
}