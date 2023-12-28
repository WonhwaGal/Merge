using System;
using UnityEngine;

namespace Code.Pools
{
    public class FXView : MonoBehaviour, ISpawnable
    {
        [SerializeField] private PrefabType _type;

        public PrefabType Type => _type;

        public event Action<PrefabType, FXView> OnDisabled;

        private void OnDisable()
        {
            OnDisabled?.Invoke(_type, this);
            OnDisabled = null;
        }
    }
}