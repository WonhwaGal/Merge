using UnityEngine;

namespace Code.Pools
{
    public sealed class SingleFactory<T> : Factory<T>
        where T : MonoBehaviour, ISpawnable
    {
        public SingleFactory(T prefab) : base(prefab) { }

        public override T Create() => GameObject.Instantiate(_prefab);
    }
}