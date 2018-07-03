namespace Gonity
{
    public delegate void DelegateCall();
    public delegate void DelegateCall<T>(T param);
    public delegate void DelegateCall<T, U>(T param1, U param2);
}