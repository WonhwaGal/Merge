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
        public Button ExitButton => _exitButton;

        public event Action OnDestroyView;

        private void Start()
        {
            _exitButton.onClick.AddListener(PressQuit);
        }

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

        private void PressQuit()
        {
            if(!_showResults)
                GameEventSystem.Send(new SaveEvent(FinalScore, onlyScore: false));
            Application.Quit();
        }

        private void OnDestroy()
        {
            OnDestroyView?.Invoke();
            RetryButton.onClick.RemoveAllListeners();
            ExitButton.onClick.RemoveAllListeners();
            OnDestroyView = null;
        }
    }
}