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
        [SerializeField] private Button _exitButton;
        private int _finalScore;

        public int FinalScore
        {
            get => _finalScore;
            set
            {
                _finalScore = value;
                _scoreText.text = _finalScore.ToString();
                if (_results.activeInHierarchy)
                    OnSaveProgress?.Invoke(FinalScore, true);
            }
        }
        public int BestScore { get; set; }

        public Action<bool> OnEndGameWithRetry { get; set; } // bool = with retry
        public Action<int, bool> OnSaveProgress { get; set; }

        private void Start()
        {
            _retryButton.onClick.AddListener(PressRetry);
            _exitButton.onClick.AddListener(PressQuit);
        }

        public void ShowResults(bool toShow, int bestScore)
        {
            _bestScoreText.text = bestScore.ToString();
            _scoreText.gameObject.SetActive(toShow);
            _bestScoreText.gameObject.SetActive(toShow);
            _results.SetActive(toShow);
        }

        private void PressRetry() => OnEndGameWithRetry?.Invoke(true);

        private void PressQuit()
        {
            if (_results.activeInHierarchy)
                return;

            OnEndGameWithRetry?.Invoke(false);
            OnSaveProgress?.Invoke(FinalScore, false);
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnEndGameWithRetry = null;
            OnSaveProgress = null;
            _retryButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}