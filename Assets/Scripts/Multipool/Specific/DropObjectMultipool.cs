using Code.DropLogic;
using UnityEngine;

namespace Code.Pools
{
    public sealed class DropObjectMultipool : MultiPool<int, DropObject>
    {
        private readonly DropObjectSO _dropObjectList;

        public DropObjectMultipool(DropObjectSO list) => _dropObjectList = list;

        protected override DropObject GetPrefab(int rank) => _dropObjectList.FindObjectData(rank).DropObject;

        public override void OnSpawned(DropObject result, Vector3 targetPos)
        {
            result.transform.position = targetPos;
        }
    }
}