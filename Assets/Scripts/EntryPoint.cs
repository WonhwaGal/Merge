using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Localization.Settings;
using Code.SaveLoad;
using Code.MVC;
using GamePush;


public class EntryPoint : MonoBehaviour
{
    [SerializeField] private StartCanvas _startCanvas;
    [SerializeField] private SpriteAtlas _atlas;

    private StartController _startController;
    private SaveService _saveService;

    private IEnumerator Start()
    {
        var localeInit = LocalizationSettings.InitializationOperation;
        localeInit.Completed += _ => CreateUIController();

        var savedData = GP_Player.GetString(Constants.DropList);
        yield return new WaitWhile(() => string.IsNullOrEmpty(savedData));
        _startCanvas.ContinueButton.interactable = _saveService.LoadProgress(savedData);
    }

    private void CreateUIController()
    {
        _saveService = ServiceLocator.Container.RegisterAndAssign(new SaveService());
        _startController = new StartController();
        _startController.AddView(_startCanvas, true);
    }
}