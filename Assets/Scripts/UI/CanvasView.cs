using Code.UI;
using UnityEngine;

namespace Code.MVC
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private PauseImage _pauseImage;
        [SerializeField] private GameUIView _gameUIView;
        [SerializeField] private PauseMenuView _pauseMenuView;
        [SerializeField] private OptionsView _optionsView;

        public PauseImage PauseImage => _pauseImage;
        public GameUIView GameUIView => _gameUIView;
        public PauseMenuView LoseView => _pauseMenuView;
        public OptionsView OptionsView => _optionsView;
    }
}