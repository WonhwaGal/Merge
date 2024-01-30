using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Code.UI;

namespace Code.MVC
{
    public sealed class MenuView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI _scoreValue;
        [SerializeField] private TextMeshProUGUI _bestScoreValue;
        [SerializeField] private TextMeshProUGUI _loseText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _rewardsButton;
        [SerializeField] private BoolButton _musicButton;
        [SerializeField] private BoolButton _soundButton;
        private float _finalScore;

        public float FinalScore
        {
            get => _finalScore;
            set
            {
                _finalScore = value;
                _scoreValue.text = _finalScore.ToString();
            }
        }
        public Button RetryButton => _retryButton;
        public Button RewardsButton => _rewardsButton;
        public BoolButton MusicButton => _musicButton;
        public BoolButton SoundButton => _soundButton;

        public event Action OnDestroyView;

        public void ShowResults(bool toShow, float bestScore)
        {
            _bestScoreValue.text = bestScore.ToString();
            ShowContent(toShow);
            if (toShow)
                GameEventSystem.Send(new SaveEvent(FinalScore, onlyScore: true));
        }

        public void SetTexts(string[] texts)
        {
            _loseText.text = texts[0];
            _bestScoreText.text = texts[1];
            _retryButton.GetComponentInChildren<TextMeshProUGUI>().text = texts[2];
            _rewardsButton.GetComponentInChildren<TextMeshProUGUI>().text = texts[3];
        }

        private void ShowContent(bool showResults)
        {
            _rewardsButton.gameObject.SetActive(!showResults);
            _loseText.gameObject.SetActive(showResults);
            _scoreValue.gameObject.SetActive(showResults);
            _bestScoreValue.gameObject.SetActive(showResults);
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            RetryButton.onClick.RemoveAllListeners();
            RewardsButton.onClick.RemoveAllListeners();
            OnDestroyView = null;
        }
    }
}