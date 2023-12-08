using System;
using UnityEngine;
using Code.Pools;

namespace Code.Views
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class DropObject : MonoBehaviour, ISpawnable
    {
        [SerializeField] private int _rank;
        [SerializeField] private bool _isFinalRank;
        private Collider2D _collider;
        private bool _isMerging;

        public int Rank => _rank;
        public Rigidbody2D RB { get; private set; }
        public bool IsMerging
        {
            get => _isMerging;
            set
            {
                _isMerging = value;
                if(value)
                    _collider.isTrigger = true;
            }

        }


        public event Action<DropObject> OnDrop;
        public event Func<DropObject, DropObject, bool> OnMerge;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            RB = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            RB.gravityScale = 0;
            _collider.isTrigger = true;
            IsMerging = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isFinalRank || RB.gravityScale == 0 || IsMerging)
                return;

            if (collision.gameObject.TryGetComponent(out DropObject drop) && drop.Rank == _rank)
            {
                if (collision.transform.position.y < transform.position.y)
                {
                    IsMerging = OnMerge.Invoke(this, drop);
                    drop.IsMerging = IsMerging;
                } 
            }
        }

        public void Drop()
        {
            RB.gravityScale = 1;
            _collider.isTrigger = false;
            OnDrop?.Invoke(this);
        }
    }
}