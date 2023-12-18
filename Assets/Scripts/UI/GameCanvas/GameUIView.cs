using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MVC
{
    public class GameUIView : MonoBehaviour, IView
    {
        [SerializeField] private Image _nextImage;
        [SerializeField] private TextMeshProUGUI _scoreText;
        private int _scoreValue;

        public Sprite NextSprite { get => _nextImage.sprite; set => _nextImage.sprite = value; }

        public int Score
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
        }
    }
}