using UnityEngine;
using GamePush;

namespace Code.DropLogic
{
    public class StaticQueueView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _unlockedViews;
        private int _dropableRanks;
        private int _lastUnlockedRank;

        private void Start()
        {
            _lastUnlockedRank = _dropableRanks = GP_Variables.GetInt("DropableRanks");
            GameEventSystem.Subscribe<MergeEvent>(UnlockRankView);
            for(int i = 0; i < _unlockedViews.Length; i++)
                _unlockedViews[i].SetActive(false);
        }

        private void UnlockRankView(MergeEvent @event)
        {
            if(@event.MergingRank == _lastUnlockedRank)
            {
                var index = _lastUnlockedRank - _dropableRanks;
                _unlockedViews[index].SetActive(true);
                _lastUnlockedRank++;
            }
        }

        private void OnDestroy() => GameEventSystem.UnSubscribe<MergeEvent>(UnlockRankView);
    }
}