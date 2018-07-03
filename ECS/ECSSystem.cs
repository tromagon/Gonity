namespace Gonity
{
    public abstract class ECSSystem
    {
        private IEntityDatabase _entityDatabase;
        protected IEntityDatabase entityDatabase { get { return _entityDatabase; } }

        public void Init(IEntityDatabase entityDatabase)
        {
            _entityDatabase = entityDatabase;
        }
    }
}