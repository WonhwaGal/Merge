using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Code.DropLogic;
using Code.SaveLoad;
using GamePush;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private StartCanvas _startCanvas;
    [SerializeField] private SpriteAtlas _atlas;

    private SaveService _saveService;
    private DropService _dropService;

    private IEnumerator Start()
    {
        _startCanvas.StartNewButton.onClick.AddListener(() => LoadNewScene(withProgress: false));
        _startCanvas.ContinueButton.onClick.AddListener(() => LoadNewScene(withProgress: true));

        _saveService = ServiceLocator.Container.RegisterAndAssign(new SaveService());
        var savedData = GP_Player.GetString(Constants.DropList);
        yield return new WaitWhile(() => string.IsNullOrEmpty(savedData));
        _startCanvas.ContinueButton.interactable = _saveService.LoadProgress(savedData);
    }


    private void LoadNewScene(bool withProgress)
    {
        SceneManager.LoadSceneAsync(Constants.GameScene);
        if (withProgress)
            SceneManager.sceneLoaded += OnLoadWithProgress;
    }


    private void OnLoadWithProgress(Scene scene, LoadSceneMode mode)
    {
        _dropService = ServiceLocator.Container.RequestFor<DropService>();
        _dropService.RecreateProgress(_saveService.ProgressData);
        SceneManager.sceneLoaded -= OnLoadWithProgress;
    }

    private void OnDestroy()
    {
        _startCanvas.StartNewButton.onClick.RemoveAllListeners();
        _startCanvas.ContinueButton.onClick.RemoveAllListeners();
    }
}