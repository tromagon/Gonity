namespace Gonity
{
    public interface IViewFactoryRegister
    {
        void Register<T>(T prefab, bool needInstantiation = true) where T : UnityEngine.Object;
    }
}