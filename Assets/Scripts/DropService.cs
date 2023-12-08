using Code.Pools;
using Code.Views;
using UnityEngine;

public sealed class DropService
{
    private readonly DropObjectSO _dropSO;
    private readonly DropObjectMultipool _pool;

    public DropService(DropObjectSO dropSO)
    {
        _dropSO = dropSO;
        _pool = new DropObjectMultipool(_dropSO);
        DropQueueHandler.AssignValues(_dropSO.TotalNumber(), 6);
    }

    public DropObject CreateDropObject(Transform transform)
    {
        var result = _pool.Spawn(DropQueueHandler.ChooseRank());
        return SetUpDropObject(result, transform, true);
    }

    public bool CheckForMerge(DropObject one, DropObject two)
    {
        var result = one.Rank < DropQueueHandler.MaxRank;
        if (result)
            MergeObjects(one, two);
        return result;
    }

    public void MergeObjects(DropObject one, DropObject two)
    {
        var result = _pool.Spawn(one.Rank + 1);
        SetUpDropObject(result, two.transform, false);
        result.Drop();
        ReturnToPool(one);
        ReturnToPool(two);
    }

    private DropObject SetUpDropObject(DropObject result, Transform transform, bool setParent)
    {
        _pool.OnSpawned(result, transform);
        if (setParent)
            result.transform.SetParent(transform);
        result.OnMerge += CheckForMerge;
        result.OnDrop += ReleaseObject;
        return result;
    }

    private void ReleaseObject(DropObject obj) => _pool.ReturnToRoot(obj.Rank, obj);

    private void ReturnToPool(DropObject obj)
    {
        _pool.Despawn(obj.Rank, obj);
        obj.OnMerge -= CheckForMerge;
        obj.OnDrop -= ReleaseObject;
    }
}