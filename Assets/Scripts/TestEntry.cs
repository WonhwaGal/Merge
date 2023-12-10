using Code.DropLogic;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private DropContainer _container;
    [SerializeField] private DropObjectSO _so;
    [SerializeField] private GameUIView _canvasView;

    private DropService _dropService;
    private UIService _uiService;

    private void Awake()
    {
        _uiService = ServiceLocator.Container.RegisterAndAssign(new UIService(_so, _canvasView));
        _dropService = new DropService(_so);
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
        _uiService.Dispose();
    }
}