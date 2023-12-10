using System;
using UnityEngine;
using Code.Pools;

namespace Code.DropLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class DropObject : MonoBehaviour, ISpawnable
    {
        [SerializeField] private int _rank;
        private Collider2D _collider;
        private bool _isFinalRank;
        private bool _collisionsIgnored;

        public int Rank => _rank;
        public Rigidbody2D RB { get; private set; }
        public bool CollisionsIgnored
        {
            get => _collisionsIgnored;
            set
            {
                _collisionsIgnored = value;
                if (value)
                    _collider.isTrigger = true;
            }

        }

        public event Action<DropObject, bool> OnEndGame;
        public event Func<DropObject, DropObject, bool> OnMerge;

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _isFinalRank = _rank == Constants.TotalRanks;
        }

        private void OnEnable()
        {
            RB.gravityScale = 0;
            CollisionsIgnored = false;
            _collider.isTrigger = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (transform.position.y > Constants.LoseThreshold)
                Register(false);

            if (_isFinalRank || RB.gravityScale == 0 || CollisionsIgnored)
                return;

            _collisionsIgnored = true;
            CheckRankCollisions(collision);
        }

        private void CheckRankCollisions(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out DropObject drop) && drop.Rank == _rank)
            {
                if (drop.transform.position.y <= transform.position.y)
                {
                    CollisionsIgnored = OnMerge.Invoke(this, drop);
                    drop.CollisionsIgnored = CollisionsIgnored;
                }
            }
            else
            {
                _collisionsIgnored = false;
            }
        }

        public void Drop()
        {
            RB.gravityScale = 1;
            _collider.isTrigger = false;
        }

        public void Register(bool withRetry)
        {
            if (RB.gravityScale != 0)
                OnEndGame?.Invoke(this, withRetry);
        }

        private void OnDestroy() => OnEndGame = null;
    }
}