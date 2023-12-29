using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GamePush;
namespace Code.MVC
{
    public class GameUIView : MonoBehaviour, IView
    {
        [SerializeField] private Image _nextImage;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
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

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            _rewardButton.onClick.RemoveAllListeners();
            _leaderBoardButton.onClick.RemoveAllListeners();
        }
    }
}