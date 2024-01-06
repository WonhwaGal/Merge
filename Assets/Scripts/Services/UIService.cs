using System;
using Code.MVC;

public sealed class UIService : IService, IDisposable
{
    public UIService(DropObjectSO data, CanvasView canvasView)
    {
        CreateControllers(data, canvasView);
    }

    public event Action OnContinueSavedGame;

    private void CreateControllers(DropObjectSO data, CanvasView canvasView)
    {
        GameUIController gameUIController = new(data);
        ((IController)gameUIController).AddView(canvasView.GameUIView, true);
        MenuController menuController = new();
        ((IController)menuController).AddView(canvasView.LoseView, false);
        SetConnections(gameUIController, menuController);
    }

    private void SetConnections(GameUIController gameUIController, MenuController menuController)
    {
        menuController.OnRequestScore += gameUIController.GetScore;
        OnContinueSavedGame += gameUIController.SetScore;
    }

    public void SetCurrentScore() => OnContinueSavedGame?.Invoke();

    public void Dispose()
    {
        OnContinueSavedGame = null;
        GC.SuppressFinalize(this);
    }
}