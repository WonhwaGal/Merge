using System;
using Code.Pools;
using UnityEngine;

namespace Code.DropLogic
{
    public sealed class DropService
    {
        private readonly DropObjectSO _dropSO;
        private readonly DropObjectMultipool _pool;
        private readonly UIService _uiService;

        public DropService(DropObjectSO dropSO)
        {
            _dropSO = dropSO;
            _pool = new DropObjectMultipool(_dropSO);
            DropQueueHandler.AssignValues(_dropSO.TotalNumber(), 6);
            _uiService = ServiceLocator.Container.RequestFor<UIService>();
        }

        public event Action<bool, int> OnCreateDropObject;

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
            Debug.Log($"merging rank {one.Rank} into {result.Rank}");
            result.Drop();
            ReturnToPool(one);
            ReturnToPool(two);
        }

        private DropObject SetUpDropObject(DropObject result, Transform transform, bool queueMoved)
        {
            _pool.OnSpawned(result, transform);
            result.OnMerge += CheckForMerge;
            result.OnEndGame += EndSession;
            _uiService.PauseView.OnEndGameWithRetry += result.Register;
            OnCreateDropObject?.Invoke(queueMoved, result.Rank);
            return result;
        }

        private void EndSession(DropObject obj, bool withRetry)
        {
            if (withRetry)
                ReturnToPool(obj);
            else
                _uiService.UpdateCanvas();
        }

        private void ReturnToPool(DropObject obj)
        {
            _pool.Despawn(obj.Rank, obj);
            obj.OnMerge -= CheckForMerge;
            obj.OnEndGame -= EndSession;
        }
    }
}