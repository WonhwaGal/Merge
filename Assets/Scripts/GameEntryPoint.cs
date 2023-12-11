using Code.DropLogic;
using Code.SaveLoad;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private DropContainer _container;
    [SerializeField] private DropObjectSO _so;
    [SerializeField] private GameUIView _canvasView;

    private SaveService _saveService;
    private UIService _uiService;
    private DropService _dropService;

    private void Awake()
    {
        _saveService = ServiceLocator.Container.RequestFor<SaveService>();
        _uiService = ServiceLocator.Container.RegisterAndAssign(new UIService(_so, _canvasView));
        _dropService = ServiceLocator.Container.RegisterAndAssign(new DropService(_so));
        SetUpConnections();
    }

    private void SetUpConnections()
    {
        _container.OnObjectDrop += _dropService.CreateDropObject;
        _dropService.OnCreateDropObject += _uiService.ChangeNextDropIcon;
        _dropService.OnRegisterDropObject += _saveService.GatherData;
        _container.CurrentDrop = _dropService.CreateDropObject(_container.transform);
    }

    private void OnDestroy()
    {
        _container.OnObjectDrop -= _dropService.CreateDropObject;
        _dropService.OnCreateDropObject -= _uiService.ChangeNextDropIcon;
        _dropService.OnRegisterDropObject -= _saveService.GatherData;
        _uiService.Dispose();
    }
}