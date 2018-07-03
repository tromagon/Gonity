using System.Collections.Generic;

namespace Gonity
{
    public class DirectCommandMap : IDirectCommandMap
    {
        private IInjector _injector;
        private List<ICommand> _commands;

        public DirectCommandMap(IInjector injector = null)
        {
            _injector = injector;
            _commands = new List<ICommand>();
        }

        public void Map(ICommand command)
        {
            _commands.Add(command);
            if (_injector != null)
            {
                _injector.Inject(command, InjectionMode.ResolveOnly);
            }
        }

        public void Execute()
        {
            var enumerator = _commands.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Execute();
            }
        }
    }
}