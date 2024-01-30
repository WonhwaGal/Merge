using System;

namespace Code.MVC
{
    public interface IView
    {
        event Action OnDestroyView;
        void SetTexts(string[] texts);
    }
}