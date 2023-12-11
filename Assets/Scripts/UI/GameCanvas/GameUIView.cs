using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Code.MVC;

public class GameUIView : MonoBehaviour, IView
{
    [SerializeField] private Image _nextImage;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private PauseImage _pauseImage;
    [SerializeField] private MenuView _loseView;

    private int _scoreValue;

    public Sprite NextSprite { get => _nextImage.sprite; set => _nextImage.sprite = value; }
    public MenuView LoseView => _loseView;
    public PauseImage PauseImage => _pauseImage;
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