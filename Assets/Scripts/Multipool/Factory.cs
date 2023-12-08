using UnityEngine;

namespace Code.Pools
{
    public abstract class Factory<T>
        where T : MonoBehaviour
    {
        protected T _prefab;

        public Transform RootTransform { get; set; }

        public Factory(T prefab) => _prefab = prefab;

        public abstract T Create();
    }
}