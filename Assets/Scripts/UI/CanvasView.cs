using Code.UI;
using UnityEngine;

namespace Code.MVC
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private PauseImage _pauseImage;
        [SerializeField] private GameUIView _gameUIView;
        [SerializeField] private MenuView _loseView;
        [SerializeField] private OptionsView _optionsView;

        public PauseImage PauseImage => _pauseImage;
        public GameUIView GameUIView => _gameUIView;
        public MenuView LoseView => _loseView;
        public OptionsView OptionsView => _optionsView;
    }
}