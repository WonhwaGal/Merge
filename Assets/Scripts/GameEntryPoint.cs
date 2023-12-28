using UnityEngine;
using Code.MVC;
using Code.SaveLoad;
using Code.DropLogic;
using UnityEngine.U2D;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private DropContainer _container;
    [SerializeField] private DropObjectSO _so;
    [SerializeField] private EffectList _fxList;
    [SerializeField] private CanvasView _canvasView;
    [SerializeField] private SpriteAtlas _atlas;
    
    private SaveService _saveService;
    private UIService _uiService;
    private DropService _dropService;

    private void Awake()
    {
        InitServices();
        SetUpConnections();
    }

    private void InitServices()
    {
        _saveService = ServiceLocator.Container.RequestFor<SaveService>();
        _uiService = ServiceLocator.Container.RegisterAndAssign(new UIService(_so, _canvasView));
        _dropService = ServiceLocator.Container.RegisterAndAssign(new DropService(_so, _fxList));
    }

    private void SetUpConnections()
    {
        _container.OnObjectDrop += _dropService.CreateDropObject;
        _container.CurrentDrop = _dropService.CreateDropObject(_container.transform, true);
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        GameEventSystem.Send(new ManageDropEvent((DropObject)_container.CurrentDrop, true));
    //        _container.CurrentDrop = Instantiate(_so.Bomb, _container.transform.position, Quaternion.identity);
    //    }
    //}

    private void OnDestroy()
    {
        _container.OnObjectDrop -= _dropService.CreateDropObject;
        _uiService.Dispose();
        _saveService.Dispose();
    }
}