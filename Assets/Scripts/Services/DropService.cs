using System;
using UnityEngine;
using Code.Pools;
using Code.SaveLoad;

namespace Code.DropLogic
{
    public sealed class DropService : IService
    {
        private readonly FXMultiPool _fxPool;
        private readonly DropObjectMultipool _pool;
        private readonly UIService _uiService;

        public DropService(DropObjectSO dropSO, EffectList fxList)
        {
            _pool = new (dropSO);
            _fxPool = new(fxList);
            DropQueueHandler.AssignValues(dropSO.TotalNumber(), Constants.DropableRanksNumber);
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

        public DropBase CreateDropObject(Transform transform, bool random)
        {
            DropBase result;
            if (random)
                result = _pool.Spawn(DropQueueHandler.ChooseRank());
            else
                result = _pool.Spawn(Constants.BombRank);
            return SetUpDropObject(result, transform.position, true, false);
        }


        public bool CheckForMerge(DropBase one, DropBase two)
        {
            var finalRank = one.Rank == DropQueueHandler.MaxRank;
            if (finalRank)
                ReturnPairToPool(one, two);
            else
                MergeObjects(one, two);
            return finalRank;
        }

        public void MergeObjects(DropBase upperOne, DropBase lowerOne)
        {
            var result = _pool.Spawn(upperOne.Rank + 1);
            var middlePos = (upperOne.transform.position + lowerOne.transform.position) / 2;
            AddEffect(PrefabType.PoofEffect, result, middlePos);
            SetUpDropObject(result, middlePos, false, true);
            ReturnPairToPool(upperOne, lowerOne);
            if (upperOne.Rank >= Constants.DropableRanksNumber)
                GameEventSystem.Send(new MergeEvent(upperOne.Rank));
        }

        private void AddEffect(PrefabType effectType, DropBase result, Vector3 middlePos)
        {
            var effect = _fxPool.Spawn(effectType);
            _fxPool.OnSpawned(effect, middlePos);
            effect.transform.localScale = result.transform.localScale;
        }

        private DropBase SetUpDropObject(DropBase result, Vector3 position, bool queueMoved, bool shouldDrop)
        {
            _pool.OnSpawned(result, position);
            result.OnMerge += CheckForMerge;
            GameEventSystem.Send(new CreateDropEvent(queueMoved, result.Rank));
            if (shouldDrop)
                result.Drop();
            return result;
        }

        private void EndSession(ManageDropEvent @event)
        {
            if (@event.ReturnToPool)
                ReturnToPool(@event.Drop);
            if(@event.WithEffects)
                AddEffect(PrefabType.PoofEffect, @event.Drop, @event.Drop.Pos);
        }

        private void ReturnPairToPool(DropBase one, DropBase two)
        {
            ReturnToPool(one);
            ReturnToPool(two);
        }

        private void ReturnToPool(DropBase result)
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