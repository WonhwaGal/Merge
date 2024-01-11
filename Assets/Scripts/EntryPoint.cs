using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using Code.SaveLoad;
using GamePush;
using Code.MVC;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private StartCanvas _startCanvas;
    [SerializeField] private SpriteAtlas _atlas;

    private StartController _startController;
    private SaveService _saveService;
    //private DropService _dropService;

    private IEnumerator Start()
    {
        _saveService = ServiceLocator.Container.RegisterAndAssign(new SaveService());
        //_startCanvas.StartNewButton.onClick.AddListener(() => LoadNewScene(withProgress: false));
        //_startCanvas.ContinueButton.onClick.AddListener(() => LoadNewScene(withProgress: true));
        _startController = new StartController();
        _startController.AddView(_startCanvas, true);

        var savedData = GP_Player.GetString(Constants.DropList);
        yield return new WaitWhile(() => string.IsNullOrEmpty(savedData));
        _startCanvas.ContinueButton.interactable = _saveService.LoadProgress(savedData);
    }


    //private void LoadNewScene(bool withProgress)
    //{
    //    SceneManager.LoadSceneAsync(Constants.GameScene);
    //    if (withProgress)
    //        SceneManager.sceneLoaded += OnLoadWithProgress;
    //}


    //private void OnLoadWithProgress(Scene scene, LoadSceneMode mode)
    //{
    //    _dropService = ServiceLocator.Container.RequestFor<DropService>();
    //    _dropService.RecreateProgress(_saveService.ProgressData);
    //    SceneManager.sceneLoaded -= OnLoadWithProgress;
    //}

    //private void OnDestroy()
    //{
    //    _startCanvas.StartNewButton.onClick.RemoveAllListeners();
    //    _startCanvas.ContinueButton.onClick.RemoveAllListeners();
    //}
}