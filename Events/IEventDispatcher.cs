using Gonity.Core;
using System;

namespace Gonity
{
    public interface IEventDispatcher
    {
        void Add(Enum eventId, Callback callback);
        void Add<T>(Enum eventId, Callback<T> callback);
        void Add<T, U>(Enum eventId, Callback<T, U> callback);
        void Add<T, U, V>(Enum eventId, Callback<T, U, V> callback);
        void Add(Enum eventId, EventCallback callback);
        void Add<T>(Enum eventId, EventCallback<T> callback);
        void Add<T, U>(Enum eventId, EventCallback<T, U> callback);
        void Add<T, U, V>(Enum eventId, EventCallback<T, U, V> callback);
        void Remove(Enum eventId, Callback callback);
        void Remove<T>(Enum eventId, Callback<T> callback);
        void Remove<T, U>(Enum eventId, Callback<T, U> callback);
        void Remove<T, U, V>(Enum eventId, Callback<T, U, V> callback);
        void Remove(Enum eventId, EventCallback callback);
        void Remove<T>(Enum eventId, EventCallback<T> callback);
        void Remove<T, U>(Enum eventId, EventCallback<T, U> callback);
        void Remove<T, U, V>(Enum eventId, EventCallback<T, U, V> callback);
        void RemoveAll();
        void Dispatch(Enum eventId);
        void Dispatch<T>(Enum eventId, T param);
        void Dispatch<T, U>(Enum eventId, T param1, U param2);
        void Dispatch<T, U, V>(Enum eventId, T param1, U param2, V param3);
    }
}