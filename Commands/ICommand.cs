namespace Gonity
{
    public interface ICoreCommand
    {
    }

    public interface ICommand : ICoreCommand
    {
        void Execute();
    }

    public interface ICommand<T> : ICoreCommand
    {
        void Execute(T param);
    }

    public interface ICommand<T, U> : ICoreCommand
    {
        void Execute(T param1, U param2);
    }

    public interface ICommand<T, U, V> : ICoreCommand
    {
        void Execute(T param1, U param2, V param3);
    }
}