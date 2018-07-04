using System;

namespace Gonity
{
    public interface IEventCommandMap
    {
        void Map(Enum eventType, ICoreCommand command);
        void Map<T>(Enum eventType, ICommand<T> command);
        void Map<T, U>(Enum eventType, ICommand<T, U> command);
        void Map<T, U, V>(Enum eventType, ICommand<T, U, V> command);
        void ClearAll();
    }
}