using System.Collections;
using UnityEngine;
using Code.MVC;
using Code.SaveLoad;
using Code.DropLogic;
using GamePush;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private DropContainer _container;
    [SerializeField] private DropObjectSO _so;
    [SerializeField] private CanvasView _canvasView;

    private SaveService _saveService;
    private UIService _uiService;
    private DropService _dropService;

    private void Awake()
    {
        //yield return null;
        //var save = GP_Player.GetString("save");//1sec
        //yield return new WaitWhile(()=> string.IsNullOrEmpty(save));
        //do 1sec late

        //float score = GP_Player.GetScore();
        //score++;
        //GP_Player.SetScore(score);
        //GP_Player.Sync();
        //var value = GP_Player.GetString("save");
        ////vlaue===
        //GP_Player.Set()
        _saveService = ServiceLocator.Container.RequestFor<SaveService>();
        _uiService = ServiceLocator.Container.RegisterAndAssign(new UIService(_so, _canvasView));
        _dropService = ServiceLocator.Container.RegisterAndAssign(new DropService(_so));
        SetUpConnections();
    }

    private void SetUpConnections()
    {
        _container.OnObjectDrop += _dropService.CreateDropObject;
        _container.CurrentDrop = _dropService.CreateDropObject(_container.transform);
    }

    private void OnDestroy()
    {
        _container.OnObjectDrop -= _dropService.CreateDropObject;
        _uiService.Dispose();
        _saveService.Dispose();
    }
}