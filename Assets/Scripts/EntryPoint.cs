using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using Code.SaveLoad;
using Code.MVC;
using Code.DropLogic;
using GamePush;
using Code.Achievements;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private StartCanvas _startCanvas;
    [SerializeField] private AchievSO _achievSO;
    [SerializeField] private SpriteAtlas _atlas;

    private SaveService _saveService;
    private AchievementService _achievementService;
    private LanguageHandler _languageHandler;

    private IEnumerator Start()
    {
        _startCanvas.StartNewButton.onClick.AddListener(() => LoadNewScene(withProgress: false));
        _startCanvas.ContinueButton.onClick.AddListener(() => LoadNewScene(withProgress: true));

        var localeInit = LocalizationSettings.InitializationOperation;
        localeInit.Completed += _ => Init();
        yield return new WaitWhile(() => _saveService == null);

        var savedData = GP_Player.GetString(Constants.DropList);
        yield return new WaitWhile(() => string.IsNullOrEmpty(savedData));
        _startCanvas.ContinueButton.interactable = _saveService.LoadProgress(savedData);
    }

    private void Init()
    {
        _saveService = ServiceLocator.Container.RegisterAndAssign(new SaveService());
        _achievementService = ServiceLocator.Container.RegisterAndAssign(new AchievementService(_achievSO));

        _languageHandler = new LanguageHandler();
        _languageHandler.OnLanguageChanged += _startCanvas.SetTexts;
        _languageHandler.UpdateLangInfo();
    }

    public void LoadNewScene(bool withProgress)
    {
        SceneManager.LoadSceneAsync(Constants.GameScene);
        if (withProgress)
            SceneManager.sceneLoaded += OnLoadWithProgress;
        else
            SceneManager.sceneLoaded += OnLoadNewGame;
    }

    private void OnLoadWithProgress(Scene scene, LoadSceneMode mode)
    {
        ServiceLocator.Container.RequestFor<DropService>().RecreateProgress(_saveService.ProgressData);
        _achievementService.SetInitialProgress(toZero: false);
        SceneManager.sceneLoaded -= OnLoadWithProgress;
    }

    private void OnLoadNewGame(Scene scene, LoadSceneMode mode)
    {
        _achievementService.SetInitialProgress(toZero: true);
        SceneManager.sceneLoaded -= OnLoadNewGame;
    }

    private void OnDestroy() 
        => _languageHandler.OnLanguageChanged -= _startCanvas.SetTexts;
}