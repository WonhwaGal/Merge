using System.Collections;
using System;
using GamePush;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public class GameUIView : MonoBehaviour, IView
    {
        [SerializeField] private Image _nextImage;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _ratingText;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private Button _achievementButton;
        [SerializeField] private TextMeshProUGUI _nextText;
        private float _scoreValue;
        private float _highlightTime;

        public Button RewardButton => _rewardButton;
        public Button LeaderBoardButton => _leaderBoardButton;
        public Button AchievementButton => _achievementButton;
        public Sprite NextSprite { get => _nextImage.sprite; set => _nextImage.sprite = value; }
        public float Score
        {
            get => _scoreValue;
            set
            {
                _scoreValue = value;
                _scoreText.text = _scoreValue.ToString();
            }
        }

        public event Action OnDestroyView;

        private void OnEnable() => _rewardButton.interactable = false;
        private void Start() => _highlightTime = GP_Variables.GetFloat("HighlightTime");

        public void SetRating(int rating)
            => _ratingText.text = rating != 0 ? rating.ToString() : string.Empty;

        public void SetTexts(string text) => _nextText.text = text;

        public void ActivateRewardButton(bool active)
        {
            RewardButton.interactable = active;
            if (active)
                StartCoroutine(ShowRewardAvailable());
        }

        private IEnumerator ShowRewardAvailable()
        {
            float count = 0;
            while (count < _highlightTime)
            {
                if (Time.deltaTime != 0)
                {
                    count += Time.deltaTime;
                    _rewardButton.transform.localScale = Vector3.one * (Mathf.PingPong(count, 0.5f) + 1);
                    yield return null;
                }
            }
            _rewardButton.transform.localScale = Vector3.one;
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            _rewardButton.onClick.RemoveAllListeners();
            _leaderBoardButton.onClick.RemoveAllListeners();
            _achievementButton.onClick.RemoveAllListeners();
        }
    }
}