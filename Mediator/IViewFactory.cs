using System;

namespace Gonity
{
    public interface IViewFactory
    {
        object GetView(Type viewType);
        void ReleaseView(object viewType);
    }
}