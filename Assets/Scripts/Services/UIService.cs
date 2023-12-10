using System;
using Code.MVC;

public class UIService : IService, IDisposable
{
    private readonly GameUIController _gameUIController;
    private readonly MenuController _menuController;
    private readonly SaveService _saveService;

    public UIService(DropObjectSO data, GameUIView canvasView)
    {
        _gameUIController = new(data);
        ((IController)_gameUIController).AddView(canvasView, true);
        _menuController = new MenuController();
        ((IController)_menuController).AddView(canvasView.LoseView, false);
        SetConnections();
        _saveService = ServiceLocator.Container.RegisterAndAssign(new SaveService());
    }

    public PauseView PauseView => _menuController.View;

    private void SetConnections()
    {
        _menuController.OnRequestScore += GetScore;
        _gameUIController.View.PauseImage.OnPressPause += _menuController.ShowPauseView;
        _menuController.View.OnEndGameWithRetry += ReactToRetry;
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

    private int GetScore() => _gameUIController.View.Score;
    public void Dispose()
    {
        _menuController.OnRequestScore -= GetScore;
        _gameUIController.View.PauseImage.OnPressPause -= _menuController.ShowPauseView;
        _menuController.View.OnEndGameWithRetry -= ReactToRetry;
    }
}