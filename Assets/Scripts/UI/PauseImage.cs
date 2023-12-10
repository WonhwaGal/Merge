using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseImage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _playImage;
    private bool _isPaused;
    private bool _isDeactivated;

    public event Action<bool> OnPressPause;

    private void Start() => _playImage.gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isDeactivated)
            UpdatePause(true);
    }

    public void UpdatePause(bool toNotify)
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        _playImage.gameObject.SetActive(_isPaused);
        _isDeactivated = !toNotify;
        if (toNotify)
            OnPressPause?.Invoke(_isPaused);
    }
}