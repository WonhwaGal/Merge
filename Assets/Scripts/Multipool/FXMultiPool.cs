using System.Collections.Generic;
using UnityEngine;

namespace Code.Pools
{
    public class FXMultiPool: MultiPool<PrefabType, FXView>
    {
        private readonly EffectList _prefabList;

        public FXMultiPool(EffectList list) => _prefabList = list;

        protected override FXView GetPrefab(PrefabType type) => _prefabList.FindType(type).View;

        public override void OnSpawned(FXView result, Vector3 targetPos)
        {
            result.transform.position = targetPos;
            result.OnDisabled += Despawn;
        }
    }
}