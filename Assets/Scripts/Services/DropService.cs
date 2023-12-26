using System;
using UnityEngine;
using Code.Pools;
using Code.SaveLoad;

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
            DropQueueHandler.AssignValues(_dropSO.TotalNumber(), Constants.DropableRanksNumber);
            _uiService = ServiceLocator.Container.RequestFor<UIService>();
            GameEventSystem.Subscribe<ManageDropEvent>(EndSession);
        }

        public void RecreateProgress(ProgressData data)
        {
            for (int i = 0; i < data.SavedDropList.Count; i++)
            {
                var result = _pool.Spawn(data.SavedDropList[i].Rank);
                SetUpDropObject(result, data.SavedDropList[i].Position, true, true);
            }
            _uiService.SetCurrentScore();
        }

        public DropObject CreateDropObject(Transform transform)
        {
            var result = _pool.Spawn(DropQueueHandler.ChooseRank());
            return SetUpDropObject(result, transform.position, true, false);
        }

        public bool CheckForMerge(DropObject one, DropObject two)
        {
            var finalRank = one.Rank == DropQueueHandler.MaxRank;
            if (finalRank)
                ReturnPairToPool(one, two);
            else
                MergeObjects(one, two);
            return finalRank;
        }

        public void MergeObjects(DropObject upperOne, DropObject lowerOne)
        {
            var result = _pool.Spawn(upperOne.Rank + 1);
            var middlePos = (upperOne.transform.position + lowerOne.transform.position) / 2;
            SetUpDropObject(result, middlePos, false, true);
            ReturnPairToPool(upperOne, lowerOne);
            if (upperOne.Rank >= Constants.DropableRanksNumber)
                GameEventSystem.Send(new MergeEvent(upperOne.Rank));
        }

        private DropObject SetUpDropObject(DropObject result, Vector3 position, bool queueMoved, bool shouldDrop)
        {
            _pool.OnSpawned(result, position);
            result.OnMerge += CheckForMerge;
            GameEventSystem.Send(new CreateDropEvent(queueMoved, result.Rank));
            if (shouldDrop)
                result.Drop(shouldDrop);
            return result;
        }

        private void EndSession(ManageDropEvent @event)
        {
            if (@event.ReturnToPool)
                ReturnToPool(@event.Drop);
        }

        private void ReturnPairToPool(DropObject one, DropObject two)
        {
            ReturnToPool(one);
            ReturnToPool(two);
        }

        private void ReturnToPool(DropObject result)
        {
            _pool.Despawn(result.Rank, result);
            result.OnMerge -= CheckForMerge;
        }

        public void Dispose()
        {
            GameEventSystem.Subscribe<ManageDropEvent>(EndSession);
            GC.SuppressFinalize(this);
        }
    }
}