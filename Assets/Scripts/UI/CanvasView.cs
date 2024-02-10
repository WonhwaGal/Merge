using Code.UI;
using UnityEngine;

namespace Code.MVC
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private PauseImage _pauseImage;
        [SerializeField] private GameUIView _gameUIView;
        [SerializeField] private PauseMenuView _pauseView;
        [SerializeField] private LoseMenuView _loseView;
        [SerializeField] private OptionsView _optionsView;

        public PauseImage PauseImage => _pauseImage;
        public GameUIView GameUIView => _gameUIView;
        public OptionsView OptionsView => _optionsView;
        public PauseMenuView PauseNew => _pauseView;
        public LoseMenuView LoseView => _loseView;
    }
}