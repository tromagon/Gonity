using System;

namespace Gonity
{
    public interface INavigator
    {
        void Register<T>(object viewType, OpenMode openMode = OpenMode.None);
        void Open(object viewType, Argument argument = null);
        void GoBack();
    }
}