using Code.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views
{
    public class RoomPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeRoomButton;
        [SerializeField] private GameObject[] _roomObjects;
        private SaveService _saveService;

        private void Awake()
        {
            _saveService = ServiceLocator.Container.RequestFor<SaveService>();
            GameEventSystem.Subscribe<GameControlEvent>(OnGameEvent);
            _closeRoomButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            for (int i = 1; i < _roomObjects.Length; i++)
                _roomObjects[i].SetActive(_saveService.Actives.Actives.Contains(i));
        }

        private void OnGameEvent(GameControlEvent @event)
        {
            if (@event.ActionToDo == GameAction.Play)
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameEventSystem.Subscribe<GameControlEvent>(OnGameEvent);
            _closeRoomButton.onClick.RemoveAllListeners();
        }
    }
}