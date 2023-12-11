using System;
using Code.MVC;

public sealed class UIService : IService, IDisposable
{
    private readonly GameUIController _gameUIController;
    private readonly MenuController _menuController;

    public UIService(DropObjectSO data, GameUIView canvasView)
    {
        _gameUIController = new(data);
        ((IController)_gameUIController).AddView(canvasView, true);
        _menuController = new MenuController();
        ((IController)_menuController).AddView(canvasView.LoseView, false);
        SetConnections();
    }

    public MenuView PauseView => _menuController.View;
    public bool GameLost => _menuController.GameIsLost;

    private void SetConnections()
    {
        _menuController.OnRequestScore += GetScore;
        _menuController.View.OnEndGameWithRetry += ReactToRetry;
        _gameUIController.View.PauseImage.OnPressPause += _menuController.ShowPauseView;
        _menuController.AssignView();
    }

    public void ChangeNextDropIcon(bool queueMoved, int mergeResult)
        => _gameUIController.UpdateUIData(queueMoved, mergeResult);

    public void UpdateCanvas()
    {
        if (_menuController.View.gameObject.activeSelf)
            return;

        _menuController.UpdateView();
        _gameUIController.View.PauseImage.UpdatePause(false);
    }

    public void ReactToRetry(bool withRetry)
    {
        if(withRetry)
            _gameUIController.ReactToRetry();
    }

    public void SetCurrentScore(int savedScore) => _gameUIController.SetScore(savedScore);
    private int GetScore() => _gameUIController.View.Score;
    public void Dispose()
    {
        _menuController.OnRequestScore -= GetScore;
        _menuController.View.OnEndGameWithRetry -= ReactToRetry;
        _gameUIController.View.PauseImage.OnPressPause -= _menuController.ShowPauseView;
    }
}