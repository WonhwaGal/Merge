using System.Collections.Generic;
using Code.Views;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DropObjectSO), menuName = "Scriptable/DropObjectSO")]
public class DropObjectSO : ScriptableObject
{
    [SerializeField] private List<DropObject> _dropsObjects;

    public DropObject FindObject(int rank)
    {
        DropObject result = null;
        result = _dropsObjects.Find(x => x.Rank == rank);
        if (result == null)
            Debug.LogError($"{name} : object with rank {rank} was not found");

        return result;
    }

    public int TotalNumber() => _dropsObjects.Count;
}