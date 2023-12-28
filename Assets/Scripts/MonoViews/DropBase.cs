using System;
using UnityEngine;
using Code.Pools;
using System.Collections;

namespace Code.DropLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class DropBase : MonoBehaviour, ISpawnable
    {
        [SerializeField] protected int _rank;
        private Collider2D _collider;
        protected bool _collisionsIgnored;

        public int Rank => _rank;
        public Collider2D Collider => _collider;
        public Rigidbody2D RB { get; private set; }
        public bool CollisionsIgnored
        {
            get => _collisionsIgnored;
            set
            {
                _collisionsIgnored = value;
                if (value)
                    Collider.isTrigger = true;
            }

        }
        public Vector3 Pos { get => transform.position; set => transform.position = value; }

        public event Func<DropBase, DropBase, bool> OnMerge;

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            RB.gravityScale = 0;
            CollisionsIgnored = false;
            Collider.enabled = false;
            ActivateSubscribtions();
        }

        public void Drop()
        {
            RB.gravityScale = 1;
            Collider.enabled = true;
            Collider.isTrigger = false;
            OnDrop();
        }

        protected bool CheckMergeWith(DropBase pair) => OnMerge.Invoke(this, pair);

        protected virtual void OnDrop() { }
        protected virtual void ActivateSubscribtions() { }
        protected virtual void CancelSubscribtions() { }

        private void OnDisable()
        {
            transform.rotation = Quaternion.identity;
            CancelSubscribtions();
        }

        private void OnDestroy() => OnMerge = null;
    }
}