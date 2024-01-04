using Code.DropLogic;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DropObjectSO), menuName = "Scriptable/DropObjectSO")]
public class DropObjectSO : ScriptableObject
{
    [SerializeField] private List<DropData> _dropsData;

    public int TotalNumber() => _dropsData.Count;

    public DropData FindObjectData(int rank)
    {
        var data = _dropsData.Find(x => x.DropBase.Rank == rank);
        if (data == null)
            Debug.LogError($"{name} : object with rank {rank} was not found");

        return data;
    }

    [System.Serializable]
    public sealed class DropData
    {
        public Sprite DropSprite;
        public float MergeRewardPoint;
        public DropBase DropBase;
    }
}