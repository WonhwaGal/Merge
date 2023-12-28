using UnityEngine;

namespace Code.DropLogic
{
    public interface IDropObject
    {
        int Rank { get; }
        Vector3 Pos { get; set; }
        Collider2D Collider { get; }
        Rigidbody2D RB { get; }
        bool CollisionsIgnored { get; set; }
        void Drop(bool withFX);
    }
}