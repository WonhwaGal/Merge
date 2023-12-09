namespace Code.MVC
{
    public abstract class Controller<V, M> : IController<V, M>
            where V : class, IView
            where M : class, IModel, new()
    {
        public Controller() => Model = new M();

        public V View { get; private set; }

        public M Model { get; }

        public abstract void UpdateView();

        void IController.AddView<T>(T view)
        {
            if (View != null)
                return;
            View = view as V;
            Show();
        }

        protected abstract void Show(); // = start
        protected abstract void Hide(); // = ondestroy
    }
}