using UnityEngine;

namespace Code.DropLogic
{
    public static class ExplosionForce
    {
        public static void CauseKnockback(this Rigidbody2D rb, DropObject origin)
        {
            Collider2D[] colliders = new Collider2D[10];
            var number = Physics2D.OverlapCircleNonAlloc(origin.transform.position, origin.KnockbackRadius, colliders);

            for (int i = 0; i < number; i++)
            {
                Vector3 dir = (colliders[i].transform.position - origin.transform.position).normalized;
                dir *= origin.KnockbackRadius;
                if (colliders[i].TryGetComponent(out DropObject dObj))
                    dObj.RB.AddForce(dir, ForceMode2D.Impulse);
            }
        }
    }
}