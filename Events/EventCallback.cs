using System;

namespace Gonity.Core
{
    public delegate void EventCallback(Enum eventId);
    public delegate void EventCallback<T>(Enum eventId, T param);
    public delegate void EventCallback<T, U>(Enum eventId, T param1, U param2);
    public delegate void EventCallback<T, U, V>(Enum eventId, T param1, U param2, V param3);
}
