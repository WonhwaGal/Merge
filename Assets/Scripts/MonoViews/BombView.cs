using UnityEngine;

namespace Code.DropLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BombView : DropBase
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_collisionsIgnored)
                return;

            _collisionsIgnored = true;
            if (collision.gameObject.TryGetComponent(out DropObject drop))
            {
                GameEventSystem.Send(new BombEvent(drop.Rank));
                GameEventSystem.Send(new SoundEvent(SoundType.Poof, true));
                ReturnToPool();
            }
            else
            {
                _collisionsIgnored = false;
            }
        }

        protected override void OnDrop()
        {
            base.OnDrop();
            MergeCounter.BombUse();
        }

        private void ReturnToPool() => GameEventSystem.Send(new ManageDropEvent(this, true, withEffects: false));
    }
}