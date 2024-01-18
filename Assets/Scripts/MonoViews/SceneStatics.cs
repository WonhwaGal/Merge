using UnityEngine;
using GamePush;
using Code.Achievements;
using Code.SaveLoad;

namespace Code.Views
{
    public class SceneStatics : MonoBehaviour
    {
        [SerializeField] private StaticQueueView[] _statics;
        [SerializeField] private int _mobileVersionIndex;
        [SerializeField] private int _desktopVersionIndex;
        private StaticQueueView _currentQueue;

        [Header("BackGrounds")]
        [SerializeField] private GameObject[] _backgrounds;

        private int _dropableRanks;
        private int _lastUnlockedRank;
        private AchievementService _achievService;

        private void Awake()
        {
            for (int i = 0; i < _statics.Length; i++)
                _statics[i].gameObject.SetActive(false);
            for (int i = 0; i < _backgrounds.Length; i++)
                _backgrounds[i].gameObject.SetActive(false);

            Init();
        }

        private void Start()
        {
            SetUp();
            if (_currentQueue != null)
            {
                var lastIndex = _lastUnlockedRank - _dropableRanks;
                for (int i = 0; i < lastIndex; i++)
                    _currentQueue.LockedViews[i].SetActive(false);
            }
        }

        private void Init()
        {
            if (GP_Device.IsMobile())
                _currentQueue = _statics[_mobileVersionIndex];
            else
                _currentQueue = _statics[_desktopVersionIndex];
            _currentQueue.gameObject.SetActive(true);

            GameEventSystem.Subscribe<MergeEvent>(UnlockRankView);
            GameEventSystem.Subscribe<BackgroundEvent>(UpdateBackground);
        }

        private void SetUp()
        {
            _achievService = ServiceLocator.Container.RequestFor<AchievementService>();
            _lastUnlockedRank = GP_Player.GetInt("unlocked_ranks");
            _dropableRanks = GP_Variables.GetInt("DropableRanks");
            if (_lastUnlockedRank == 0)
                _lastUnlockedRank = _dropableRanks;
            SetUpBackground();
        }

        private void SetUpBackground()
        {
            var service = ServiceLocator.Container.RequestFor<SaveService>();
            var activeBackgroundList = service.Actives.Actives;
            for (int i = 0; i < _backgrounds.Length; i++)
                _backgrounds[i].SetActive(activeBackgroundList.Contains(i));
        }

        private void UnlockRankView(MergeEvent @event)
        {
            if (@event.MergingRank == _lastUnlockedRank)
            {
                var index = _lastUnlockedRank - _dropableRanks;
                _currentQueue.LockedViews[index].SetActive(false);
                _lastUnlockedRank++;
                GP_Player.Set("unlocked_ranks", _lastUnlockedRank);
                GP_Player.Sync();
                _achievService.CheckAchievement(AchievType.FullSetUnlock, _lastUnlockedRank);
            }
        }

        private void UpdateBackground(BackgroundEvent @event)
        {
            if (@event.Index < _backgrounds.Length)
                _backgrounds[@event.Index].SetActive(@event.ToApply);
        }

        private void OnDestroy()
        {
            GameEventSystem.UnSubscribe<MergeEvent>(UnlockRankView);
            GameEventSystem.UnSubscribe<BackgroundEvent>(UpdateBackground);
        }
    }
}