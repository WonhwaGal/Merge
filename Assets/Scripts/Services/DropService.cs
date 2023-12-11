using System;
using Code.Pools;
using Code.SaveLoad;
using UnityEngine;

namespace Code.DropLogic
{
    public sealed class DropService : IService
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
        public event Action<DropObject> OnRegisterDropObject;

        public void RecreateProgress(ProgressData data)
        {
            for (int i = 0; i < data.SavedDropList.Count; i++)
            {
                var result = _pool.Spawn(data.SavedDropList[i].Rank);
                SetUpDropObject(result, data.SavedDropList[i].Position, true);
                result.Drop();
            }
            _uiService.SetCurrentScore(data.SavedScore);
        }

        public DropObject CreateDropObject(Transform transform)
        {
            var result = _pool.Spawn(DropQueueHandler.ChooseRank());
            return SetUpDropObject(result, transform.position, true);
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
            SetUpDropObject(result, two.transform.position, false);
            result.Drop();
            ReturnToPool(one);
            ReturnToPool(two);
        }

        private DropObject SetUpDropObject(DropObject result, Vector3 position, bool queueMoved)
        {
            _pool.OnSpawned(result, position);
            result.OnMerge += CheckForMerge;
            result.OnEndGame += EndSession;
            _uiService.PauseView.OnEndGameWithRetry += result.Register;
            OnCreateDropObject?.Invoke(queueMoved, result.Rank);
            return result;
        }

        private void EndSession(DropObject obj, bool withRetry)
        {
            if (withRetry)
            {
                ReturnToPool(obj);
                return;
            }

            _uiService.UpdateCanvas();
            if (!_uiService.GameLost)
                OnRegisterDropObject?.Invoke(obj);
        }

        private void ReturnToPool(DropObject obj)
        {
            _pool.Despawn(obj.Rank, obj);
            obj.OnMerge -= CheckForMerge;
            obj.OnEndGame -= EndSession;
            _uiService.PauseView.OnEndGameWithRetry -= obj.Register;
        }
    }
}