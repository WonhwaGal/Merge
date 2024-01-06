using System.Collections;
using System;
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
        private float _scoreValue;

        public Button RewardButton => _rewardButton;
        public Button LeaderBoardButton => _leaderBoardButton;
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

        public void SetRating(int rating)
            => _ratingText.text = rating != 0 ? rating.ToString() : string.Empty;

        public void ActivateRewardButton(bool active)
        {
            RewardButton.interactable = active;
            if (active)
                StartCoroutine(ShowRewardAvailable());
        }

        private IEnumerator ShowRewardAvailable()
        {
            float count = 0;
            while (count < Constants.HighlightTime)
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
        }
    }
}