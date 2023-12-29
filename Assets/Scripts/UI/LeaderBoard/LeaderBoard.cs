using System;
using UnityEngine;

namespace Code.MVC
{
    public class LeaderBoard : MonoBehaviour, IView
    {
        public event Action OnDestroyView;
    }
}