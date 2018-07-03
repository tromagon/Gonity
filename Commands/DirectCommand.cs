namespace Gonity
{
    public class DirectCommand : IDirectCommand
    {
        private IInjector _injector;

        public DirectCommand(IInjector injector = null)
        {
            _injector = injector;
        }

        void IDirectCommand.Execute(ICommand command)
        {
            if (_injector != null)
            {
                _injector.Inject(command, InjectionMode.ResolveOnly);
            }

            command.Execute();
        }
    }
}

