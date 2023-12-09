using Code.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasView : MonoBehaviour, IView
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

}