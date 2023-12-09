﻿using System.Collections.Generic;
using Code.Views;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DropObjectSO), menuName = "Scriptable/DropObjectSO")]
public class DropObjectSO : ScriptableObject
{
    [SerializeField] private List<DropData> _dropsData;

    public int TotalNumber() => _dropsData.Count;

    public DropData FindObjectData(int rank)
    {
        var data = _dropsData.Find(x => x.DropObject.Rank == rank);
        if (data == null)
            Debug.LogError($"{name} : object with rank {rank} was not found");

        return data;
    }

    [System.Serializable]
    public class DropData
    {
        public Sprite DropSprite;
        public int MergeRewardPoint;
        public DropObject DropObject;
    }
}