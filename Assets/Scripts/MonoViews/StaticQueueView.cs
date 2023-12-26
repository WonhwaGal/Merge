using UnityEngine;

namespace Code.DropLogic
{
    public class StaticQueueView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _unlockedViews;
        private int _lastUnlockedRank;

        private void Start()
        {
            _lastUnlockedRank = Constants.DropableRanksNumber;
            GameEventSystem.Subscribe<MergeEvent>(UnlockRankView);
            for(int i = 0; i < _unlockedViews.Length; i++)
                _unlockedViews[i].SetActive(false);
        }

        private void UnlockRankView(MergeEvent @event)
        {
            if(@event.MergingRank == _lastUnlockedRank)
            {
                var index = _lastUnlockedRank - Constants.DropableRanksNumber;
                _unlockedViews[index].SetActive(true);
                _lastUnlockedRank++;
            }
        }

        private void OnDestroy() => GameEventSystem.UnSubscribe<MergeEvent>(UnlockRankView);
    }
}