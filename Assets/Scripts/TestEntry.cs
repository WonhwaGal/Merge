using Code.Views;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private DropContainer _container;
    [SerializeField] private DropObjectSO _so;
    [SerializeField] private CanvasView _canvasView;

    private DropService _dropService;
    private UIService _uiService;

    private void Awake()
    {
        _dropService = new DropService(_so);
        _uiService = new UIService(_so, _canvasView);
        SetUpConnections();
    }

    private void SetUpConnections()
    {
        _container.OnObjectDrop += _dropService.CreateDropObject;
        _dropService.OnCreateDropObject += _uiService.ChangeNextDropIcon;
        _container.CurrentDrop = _dropService.CreateDropObject(_container.transform);
    }

    private void OnDestroy()
    {
        _container.OnObjectDrop -= _dropService.CreateDropObject;
        _dropService.OnCreateDropObject -= _uiService.ChangeNextDropIcon;
    }
}