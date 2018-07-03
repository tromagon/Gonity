namespace Gonity.Core
{
    public delegate void OnDestroyed(Entity entity);

    public interface IEntityCallback
    {
        void Destroy();
    }
}