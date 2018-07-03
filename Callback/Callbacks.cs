namespace Gonity.Core
{
    public delegate void Callback();
    public delegate void Callback<T>(T param);
    public delegate void Callback<T, U>(T param1, U param2);
    public delegate void Callback<T, U, V>(T param1, U param2, V param3);
}