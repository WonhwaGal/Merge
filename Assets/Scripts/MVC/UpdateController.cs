
namespace Code.MVC
{
    public abstract class UpdateController<V, M> : Controller<V, M>, IUpdatableController
            where V : class, IView
            where M : class, IModel, new()
    {
        public string Tag => GetType().Name;

        void IUpdatableController.UpdateController(string tag)
        {
            if (string.IsNullOrEmpty(tag) || tag.Equals(Tag))
                return;
            UpdateView();
        }
    }
}