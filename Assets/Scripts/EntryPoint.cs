using Code.DropLogic;
using Code.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private StartCanvas _startCanvas;

    private SaveService _saveService;
    private DropService _dropService;

    private void Start()
    {
        _saveService = ServiceLocator.Container.RegisterAndAssign(new SaveService());
        _startCanvas.ContinueButton.interactable = HasSavedProgress();
        _startCanvas.StartNewButton.onClick.AddListener(() => LoadNewScene(withProgress: false));
        _startCanvas.ContinueButton.onClick.AddListener(() => LoadNewScene(withProgress: true));
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

    private bool HasSavedProgress()
    {
        var savedGame = _saveService.LoadProgress();
        return savedGame != null && savedGame.SavedDropList.Count > 0;
    }
}