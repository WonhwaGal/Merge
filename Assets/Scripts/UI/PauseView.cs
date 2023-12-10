using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public sealed class PauseView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
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
            }
        }

        public event Action<bool> OnEndGameWithRetry; // bool = with retry

        private void Start()
        {
            _retryButton.onClick.AddListener(Retry);
            _exitButton.onClick.AddListener(Quit);
        }

        public void ShowResults(bool toShow)
        {
            _scoreText.gameObject.SetActive(toShow);
            _results.SetActive(toShow);
        }

        private void Retry() => OnEndGameWithRetry?.Invoke(true);
        private void Quit()
        {
            if (!_results.activeSelf)
                OnEndGameWithRetry?.Invoke(false);
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnEndGameWithRetry = null;
            _retryButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}