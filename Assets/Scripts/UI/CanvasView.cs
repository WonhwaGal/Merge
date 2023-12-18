using UnityEngine;

namespace Code.MVC
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private PauseImage _pauseImage;
        [SerializeField] private GameUIView _gameUIView;
        [SerializeField] private MenuView _loseView;

        public PauseImage PauseImage => _pauseImage;
        public GameUIView GameUIView => _gameUIView;
        public MenuView LoseView => _loseView;
    }
}