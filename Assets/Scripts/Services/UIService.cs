using System;
using Code.MVC;
using UnityEngine;

public sealed class UIService : IService, IDisposable
{
    public UIService(DropObjectSO data, CanvasView canvasView, AchievSO achievSO) 
        => CreateControllers(data, canvasView, achievSO);

    public event Action OnContinueSavedGame;

    private void CreateControllers(DropObjectSO data, CanvasView canvasView, AchievSO achievSO)
    {
        GameUIController gameUIC = new(data);
        gameUIC.AddView(canvasView.GameUIView, true);
        PauseMenuController pauseC = new();
        pauseC.AddView(canvasView.PauseNew, false);
        LoseMenuController loseC = new();
        loseC.AddView(canvasView.LoseView, false);
        OptionController optionsC = new(achievSO);
        optionsC.AddView(canvasView.OptionsView, false);
        SetConnections(gameUIC, pauseC, loseC, optionsC);
    }

    private void SetConnections(GameUIController gameUIC, PauseMenuController pauseC, 
        LoseMenuController loseC, OptionController optionsC)
    {
        loseC.OnRequestScore += gameUIC.GetScore;
        pauseC.OnRequestScore += gameUIC.GetScore;
        pauseC.OnRequestRewards += optionsC.UpdateView;
        OnContinueSavedGame += gameUIC.SetScore;
    }

    public void SetCurrentScore() => OnContinueSavedGame?.Invoke();

    public void Dispose()
    {
        OnContinueSavedGame = null;
        GC.SuppressFinalize(this);
    }
}