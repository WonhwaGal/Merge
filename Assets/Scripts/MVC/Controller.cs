using System;

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

        void IController.AddView<T>(T view, bool defaultShow)
        {
            if (View != null)
                return;
            View = view as V;
            View.OnDestroyView += Dispose;
            if (defaultShow)
                Show();
            else
                Hide();
        }

        protected abstract void Show();
        protected abstract void Hide();

        public virtual void Dispose()
        {
            View.OnDestroyView -= Dispose;
            GC.SuppressFinalize(this);
        }
    }
}