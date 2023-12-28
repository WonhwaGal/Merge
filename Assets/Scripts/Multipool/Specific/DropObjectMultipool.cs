using Code.DropLogic;
using UnityEngine;

namespace Code.Pools
{
    public sealed class DropObjectMultipool : MultiPool<int, DropBase>
    {
        private readonly DropObjectSO _dropObjectList;

        public DropObjectMultipool(DropObjectSO list) => _dropObjectList = list;

        protected override DropBase GetPrefab(int rank) => _dropObjectList.FindObjectData(rank).DropBase;

        public override void OnSpawned(DropBase result, Vector3 targetPos)
        {
            result.transform.position = targetPos;
        }
    }
}