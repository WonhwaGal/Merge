using Code.Views;
using UnityEngine;

namespace Code.Pools
{
    public sealed class DropObjectMultipool : MultiPool<int, DropObject>
    {
        private readonly DropObjectSO _dropObjectList;

        public DropObjectMultipool(DropObjectSO list) => _dropObjectList = list;

        protected override DropObject GetPrefab(int rank) => _dropObjectList.FindObject(rank);

        public override void OnSpawned(DropObject result, Transform targetPlace)
        {
            result.transform.position = targetPlace.position;
        }
    }
}