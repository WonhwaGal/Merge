using Code.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public sealed class MenuView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private GameObject _results;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _rewardsButton;
        [SerializeField] private BoolButton _musicButton;
        [SerializeField] private BoolButton _soundButton;
        private float _finalScore;
        private bool _showResults;

        public float FinalScore
        {
            get => _finalScore;
            set
            {
                _finalScore = value;
                _scoreText.text = _finalScore.ToString();
            }
        }
        public Button RetryButton => _retryButton;
        public Button RewardsButton => _rewardsButton;
        public BoolButton MusicButton => _musicButton;
        public BoolButton SoundButton => _soundButton;

        public event Action OnDestroyView;

        public void ShowResults(bool toShow, float bestScore)
        {
            _showResults = toShow;
            _bestScoreText.text = bestScore.ToString();
            _scoreText.gameObject.SetActive(_showResults);
            _bestScoreText.gameObject.SetActive(_showResults);
            _results.SetActive(_showResults);
            if(_showResults)
                GameEventSystem.Send(new SaveEvent(FinalScore, onlyScore: true));
        }

        public void SetTexts(string score, string best, string retry, string rewards)
        {
            _scoreText.text = score;
            _bestScoreText.text = best;
            _retryButton.GetComponentInChildren<TextMeshProUGUI>().text = retry;
            _rewardsButton.GetComponentInChildren<TextMeshProUGUI>().text = rewards;
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