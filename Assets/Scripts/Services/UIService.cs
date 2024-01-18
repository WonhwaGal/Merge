using System;
using Code.MVC;

public sealed class UIService : IService, IDisposable
{
    public UIService(DropObjectSO data, CanvasView canvasView, AchievSO achievSO) 
        => CreateControllers(data, canvasView, achievSO);

    public event Action OnContinueSavedGame;

    private void CreateControllers(DropObjectSO data, CanvasView canvasView, AchievSO achievSO)
    {
        GameUIController gameUIController = new(data);
        gameUIController.AddView(canvasView.GameUIView, true);
        MenuController menuController = new();
        menuController.AddView(canvasView.LoseView, false);
        OptionController optionsController = new(achievSO);
        optionsController.AddView(canvasView.OptionsView, false);
        SetConnections(gameUIController, menuController, optionsController);
    }

    private void SetConnections(GameUIController gameUIController, MenuController menuController, 
        OptionController optionsController)
    {
        menuController.OnRequestScore += gameUIController.GetScore;
        menuController.OnRequestOptions += optionsController.UpdateView;
        OnContinueSavedGame += gameUIController.SetScore;
    }

    public void SetCurrentScore() => OnContinueSavedGame?.Invoke();

    public void Dispose()
    {
        OnContinueSavedGame = null;
        GC.SuppressFinalize(this);
    }
}