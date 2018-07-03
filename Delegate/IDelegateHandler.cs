namespace Gonity
{
    public interface IDelegateHandler
    {
        DelegateCall Add(DelegateCall delegateCall);
        void Remove(DelegateCall delegateCall);
        void RemoveAll();
    }

    public interface IDelegateHandler<T>
    {
        void Add(DelegateCall<T> delegateCall);
        void Remove(DelegateCall<T> delegateCall);
        void RemoveAll();
    }

    public interface IDelegateHandler<T, U>
    {
        void Add(DelegateCall<T, U> delegateCall);
        void Remove(DelegateCall<T, U> delegateCall);
        void RemoveAll();
    }
}