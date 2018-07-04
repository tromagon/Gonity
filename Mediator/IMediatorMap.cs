using System;

namespace Gonity
{
    public interface IMediatorMap
    {
        void Map<T>(object viewType);
        void Open(object viewType, Argument arguments = null);
        void Close(object viewType);
        void CloseAll();
        void ClearAll();
    }
}