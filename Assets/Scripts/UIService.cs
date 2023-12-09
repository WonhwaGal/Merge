using Code.MVC;

public class UIService
{
    private readonly CanvasController _canvasController;
    public UIService(DropObjectSO data, CanvasView canvasView)
    {
        _canvasController = new CanvasController(data);
        ((IController)_canvasController).AddView(canvasView);
    }

    public void ChangeNextDropIcon(bool queueMoved, int mergeResult) 
        => _canvasController.UpdateUIData(queueMoved, mergeResult);
}