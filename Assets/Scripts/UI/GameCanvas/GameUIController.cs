using Code.MVC;

public sealed class GameUIController : Controller<GameUIView, GameUIModel>
{
    private bool _queueWasMoved;

    public GameUIController(DropObjectSO data) : base()
        => Model.AssignDataSource(data);

    public void UpdateUIData(bool queueMoved, int mergeResult)
    {
        _queueWasMoved = queueMoved;
        Model.MergedRank = mergeResult;
        UpdateView();
    }

    public void ReactToRetry()
    {
        View.PauseImage.UpdatePause(true);
        View.Score = 0;
    }

    public override void UpdateView()
    {
        if (_queueWasMoved)
            View.NextSprite = Model.GetNextRank();
        else
            View.Score += Model.GetAddPoints();
    }

    public void SetScore(int score) => View.Score = score;
    protected override void Hide() => View.gameObject.SetActive(false);
    protected override void Show() => View.gameObject.SetActive(true);
}