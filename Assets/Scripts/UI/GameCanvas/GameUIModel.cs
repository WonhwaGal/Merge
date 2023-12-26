using Code.MVC;
using UnityEngine;
using GamePush;

public class GameUIModel : IModel
{
    private DropObjectSO _dropData;

    public void AssignDataSource(DropObjectSO dropData) => _dropData = dropData;

    public int MergedRank { get; set; }

    public Sprite GetNextRank()
    {
        var nextRank = DropQueueHandler.NextDrop;
        return _dropData.FindObjectData(nextRank).DropSprite;
    }

    public float SetScore() => GP_Player.GetScore();

    public int GetAddPoints() => 
        _dropData.FindObjectData(MergedRank - 1).MergeRewardPoint;
}