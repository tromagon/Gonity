namespace Gonity
{
    public interface IDirectCommandMap : ICommand
    {
        void Map(ICommand command);
    }
}