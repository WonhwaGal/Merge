using UnityEngine;

namespace Code.Views
{
    public class StaticQueueView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _lockedViews;

        public GameObject[] LockedViews => _lockedViews;
    }
}