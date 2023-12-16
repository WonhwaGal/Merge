using System;
using UnityEngine;
using Code.Pools;

namespace Code.DropLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class DropObject : MonoBehaviour, ISpawnable
    {
        [SerializeField] private int _rank;
        [SerializeField, Range(0, 3)] 
        private float _knockbackRadius;

        private Collider2D _collider;
        private bool _isFinalRank;
        private bool _collisionsIgnored;

        public int Rank => _rank;
        public Collider2D Collider => _collider;
        public float KnockbackRadius => _knockbackRadius;
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
            Collider.enabled = false;
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
                //the upper one calls for merge
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
            Collider.enabled = true;
            Collider.isTrigger = false;
            RB.CauseKnockback(this);
        }

        public void Register(bool withRetry)
        {
            if (RB.gravityScale != 0)
                OnEndGame?.Invoke(this, withRetry);
        }

        private void OnDestroy() => OnEndGame = null;
    }
}