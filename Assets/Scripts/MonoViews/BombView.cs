using UnityEngine;

namespace Code.DropLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BombView : DropBase
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out DropObject drop))
                GameEventSystem.Send(new BombEvent(drop.Rank));
            ReturnToPool();
        }

        private void ReturnToPool() => GameEventSystem.Send(new ManageDropEvent(this, true, withEffects: false));
    }
}