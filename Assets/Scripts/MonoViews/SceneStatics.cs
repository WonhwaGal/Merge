using UnityEngine;
using GamePush;
using Code.Achievements;

namespace Code.Views
{
    public class SceneStatics : MonoBehaviour
    {
        [SerializeField] private StaticQueueView[] _statics;
        [SerializeField] private int _mobileVersionIndex;
        [SerializeField] private int _desktopVersionIndex;
        private StaticQueueView _currentQueue;

        private int _dropableRanks;
        private int _lastUnlockedRank;
        private AchievementService _achieService;
        private void Awake()
        {
            for(int i = 0; i < _statics.Length; i++)
                _statics[i].gameObject.SetActive(false);

            if (GP_Device.IsMobile())
                _currentQueue = _statics[_mobileVersionIndex];
            else
                _currentQueue = _statics[_desktopVersionIndex];
            _currentQueue.gameObject.SetActive(true);
        }


        private void Start()
        {
            SetUp();
            GameEventSystem.Subscribe<MergeEvent>(UnlockRankView);
            if(_currentQueue != null)
            {
                var startIndex = _lastUnlockedRank - _dropableRanks;
                for (int i = startIndex; i < _currentQueue.LockedViews.Length; i++)
                    _currentQueue.LockedViews[i].SetActive(false);
            }
        }

        private void SetUp()
        {
            _achieService = ServiceLocator.Container.RequestFor<AchievementService>();
            _lastUnlockedRank = GP_Player.GetInt("unlocked_ranks");
            _dropableRanks = GP_Variables.GetInt("DropableRanks");
            if (_lastUnlockedRank == 0)
                _lastUnlockedRank = _dropableRanks;
        }

        private void UnlockRankView(MergeEvent @event)
        {
            if (@event.MergingRank == _lastUnlockedRank)
            {
                var index = _lastUnlockedRank - _dropableRanks;
                _currentQueue.LockedViews[index].SetActive(true);
                _lastUnlockedRank++;
                GP_Player.Set("unlocked_ranks", _lastUnlockedRank);
                GP_Player.Sync();
                _achieService.CheckAchievement(AchievType.FullSetUnlock, _lastUnlockedRank);
            }
        }

        private void OnDestroy() => GameEventSystem.UnSubscribe<MergeEvent>(UnlockRankView);
    }
}