namespace Gonity
{
    public class Property<T>
    {
        private DelegateHandler _onChanged = new DelegateHandler();
        public IDelegateHandler onChanged { get { return _onChanged; } }

        private T _value;
        public T value
        {
            get { return _value; }
            set
            {
                _value = value;
                _onChanged.Invoke();
            }
        }
    }
}
