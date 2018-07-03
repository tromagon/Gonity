namespace Gonity
{
    public class DelegateHandler : IDelegateHandler
    {
        private DelegateCall _delegateCalls;

        DelegateCall IDelegateHandler.Add(DelegateCall delegateCall)
        {
            _delegateCalls += delegateCall;
            return delegateCall;
        }

        void IDelegateHandler.Remove(DelegateCall delegateCall)
        {
            _delegateCalls -= delegateCall;
        }

        void IDelegateHandler.RemoveAll()
        {
            _delegateCalls = null;
        }

        public void Invoke()
        {
            if (_delegateCalls != null)
            {
                _delegateCalls();
            }
        }
    }

    public class DelegateHandler<T> : IDelegateHandler<T>
    {
        private DelegateCall<T> _delegateCalls;

        void IDelegateHandler<T>.Add(DelegateCall<T> delegateCall)
        {
            _delegateCalls += delegateCall;
        }

        void IDelegateHandler<T>.Remove(DelegateCall<T> delegateCall)
        {
            _delegateCalls -= delegateCall;
        }

        void IDelegateHandler<T>.RemoveAll()
        {
            _delegateCalls = null;
        }

        public void Invoke(T param)
        {
            if (_delegateCalls != null)
            {
                _delegateCalls(param);
            }
        }
    }

    public class DelegateHandler<T, U> : IDelegateHandler<T, U>
    {
        private DelegateCall<T, U> _delegateCalls;

        void IDelegateHandler<T, U>.Add(DelegateCall<T, U> delegateCall)
        {
            _delegateCalls += delegateCall;
        }

        void IDelegateHandler<T, U>.Remove(DelegateCall<T, U> delegateCall)
        {
            _delegateCalls -= delegateCall;
        }

        void IDelegateHandler<T, U>.RemoveAll()
        {
            _delegateCalls = null;
        }

        public void Invoke(T param1, U param2)
        {
            if (_delegateCalls != null)
            {
                _delegateCalls(param1, param2);
            }
        }
    }
}