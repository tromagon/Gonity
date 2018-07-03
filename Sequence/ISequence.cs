using Gonity.Core;
using System;

namespace Gonity
{
    public interface ISequence
    {
        ISequence Call(Callback callback);
        ISequence Call<T>(Callback<T> callback, T param);
        ISequence Call<T, U>(Callback<T, U> callback, T param1, U param2);
        ISequence Call<T, U, V>(Callback<T, U, V> callback, T param1, U param2, V param3);
        ISequence WaitFor(Enum eventId, Callback callback = null);
        ISequence WaitFor<T>(Enum eventId, Callback<T> callback = null);
        ISequence WaitFor<T, U>(Enum eventId, Callback<T, U> callback = null);
        ISequence WaitFor<T, U, V>(Enum eventId, Callback<T, U, V> callback = null);
        void Start();
    }
}