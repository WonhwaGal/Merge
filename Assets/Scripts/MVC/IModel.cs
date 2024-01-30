using System;

namespace Code.MVC
{
    public interface IModel
    {
        event Action<string[]> OnLanguageChanged;
    }
}