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
        [SerializeField] private Button _bombButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _ratingPanel;
        [SerializeField] private TextMeshProUGUI _ratingText;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private TextMeshProUGUI _nextText;
        private float _scoreValue;
        private float _highlightTime;

        public Button BombButton => _bombButton;
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

        private void OnEnable() => _bombButton.interactable = false;
        private void Start() => _highlightTime = GP_Variables.GetFloat("HighlightTime");

        public void SetRating(int rating)
        {
            var toShow = rating != 0;
            _ratingPanel.SetActive(toShow);
            _ratingText.text = toShow ? rating.ToString() : string.Empty;
        }

        public void SetTexts(string[] texts) => _nextText.text = texts[0];

        public void ActivateRewardButton(bool active)
        {
            BombButton.interactable = active;
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
                    _bombButton.transform.localScale = Vector3.one * (Mathf.PingPong(count, 0.5f) + 1);
                    yield return null;
                }
            }
            _bombButton.transform.localScale = Vector3.one;
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            OnDestroyView = null;
            _bombButton.onClick.RemoveAllListeners();
            _leaderBoardButton.onClick.RemoveAllListeners();
        }
    }
}