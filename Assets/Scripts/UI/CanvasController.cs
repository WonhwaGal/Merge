using Code.MVC;

public class CanvasController : Controller<CanvasView, CanvasModel>
{
    private bool _queueWasMoved;

    public CanvasController(DropObjectSO data) : base()
        => Model.AssignDataSource(data);


    public void UpdateUIData(bool queueMoved, int mergeResult)
    {
        _queueWasMoved = queueMoved;
        Model.MergedRank = mergeResult;
        UpdateView();
    }

    public override void UpdateView()
    {
        if (_queueWasMoved)
            View.NextSprite = Model.GetNextRank();
        else
            View.Score += Model.GetAddPoints();
    }

    protected override void Hide() => View.gameObject.SetActive(false);
    protected override void Show() => View.gameObject.SetActive(true);
}