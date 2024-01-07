using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI
{
    public class BoolButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _secondImage;
        [SerializeField] private SoundType _type;
        private bool _mainIsTrue = true;

        private void Start() => _secondImage.SetActive(false);

        public void OnPointerClick(PointerEventData eventData)
        {
            _secondImage.SetActive(_mainIsTrue);
            _mainIsTrue = !_mainIsTrue;
            GameEventSystem.Send(new SoundEvent(_type, _mainIsTrue));
        }
    }
}