using System;

namespace Code.MVC
{
    public interface IController
    {
        void AddView<T>(T view, bool defaultShow) where T : class, IView;
    }

    public interface IController<V, M> : IController, IDisposable
        where V : class, IView
        where M : class, IModel
    {
        V View { get; }
        M Model { get; }
        void UpdateView();
    }
}