using UnityEngine;
using TMPro;

namespace Code.MVC
{
    public class LoseMenuView : BaseMenuView
    {
        [SerializeField] private TextMeshProUGUI _scoreValue;
        [SerializeField] private TextMeshProUGUI _bestScoreValue;
        [SerializeField] private TextMeshProUGUI _loseText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
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

        public void ShowResults(float bestScore)
        {
            _bestScoreValue.text = bestScore.ToString();
            GameEventSystem.Send(new SaveEvent(FinalScore, onlyScore: true));
        }

        public override void SetTexts(string[] texts)
        {
            _loseText.text = texts[0];
            _bestScoreText.text = texts[1];
            RetryButton.GetComponentInChildren<TextMeshProUGUI>().text = texts[2];
        }
    }
}