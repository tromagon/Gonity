namespace Gonity
{
    public interface IInjector
    {
        void Inject<T>(T instance, bool persistent = false);
        void Inject(object instance, InjectionMode mode, bool persistent = false);
        T GetInjected<T>() where T : class;
        void ClearAll(bool persistent = false);
    }
}