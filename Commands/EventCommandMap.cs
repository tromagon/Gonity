using System;
using System.Collections.Generic;

namespace Gonity
{
    public class EventCommandMap : IEventCommandMap
    {
        private IEventDispatcher _eventDispatcher;
        private IInjector _injector;
        private Dictionary<Enum, List<ICoreCommand>> _commandMap;

        public EventCommandMap(IEventDispatcher eventDispatcher, IInjector injector = null)
        {
            _eventDispatcher = eventDispatcher;
            _injector = injector;
            _commandMap = new Dictionary<Enum, List<ICoreCommand>>();
        }

        public void Map(Enum eventType, ICoreCommand command)
        {
            if (RegisterCommand(eventType, command))
            {
                _eventDispatcher.Add(eventType, OnEventReceived);
            }
        }

        public void Map<T>(Enum eventType, ICommand<T> command)
        {
            if (RegisterCommand(eventType, command))
            {
                _eventDispatcher.Add<T>(eventType, OnEventReceived);
            }
            
        }

        public void Map<T, U>(Enum eventType, ICommand<T, U> command)
        {
            if (RegisterCommand(eventType, command))
            {
                _eventDispatcher.Add<T, U>(eventType, OnEventReceived);
            }
        }

        public void Map<T, U, V>(Enum eventType, ICommand<T, U, V> command)
        {
            if (RegisterCommand(eventType, command))
            {
                _eventDispatcher.Add<T, U, V>(eventType, OnEventReceived);
            }
        }

        public void ClearAll()
        {
            _commandMap.Clear();
        }

        private bool RegisterCommand(Enum eventType, ICoreCommand command)
        {
            bool newKey = false;

            if (!_commandMap.ContainsKey(eventType))
            {
                newKey = true;
                _commandMap.Add(eventType, new List<ICoreCommand>());
            }

            _commandMap[eventType].Add(command);
            ResolveDependencies(command);

            return newKey;
        }

        private void OnEventReceived(Enum eventType)
        {
            List<ICoreCommand> commands = _commandMap[eventType];
            for (int i = 0; i < commands.Count; ++i)
            {
                (commands[i] as ICommand).Execute();
            }
        }
        
        private void OnEventReceived<T>(Enum eventType, T param)
        {
            List<ICoreCommand> commands = _commandMap[eventType];
            for (int i = 0; i < commands.Count; ++i)
            {
                (commands[i] as ICommand<T>).Execute(param);
            }
        }

        private void OnEventReceived<T, U>(Enum eventType, T param1, U param2)
        {
            List<ICoreCommand> commands = _commandMap[eventType];
            for (int i = 0; i < commands.Count; ++i)
            {
                (commands[i] as ICommand<T, U>).Execute(param1, param2);
            }
        }

        private void OnEventReceived<T, U, V>(Enum eventType, T param1, U param2, V param3)
        {
            List<ICoreCommand> commands = _commandMap[eventType];
            for (int i = 0; i < commands.Count; ++i)
            {
                (commands[i] as ICommand<T, U, V>).Execute(param1, param2, param3);
            }
        }

        private void ResolveDependencies(ICoreCommand command)
        {
            if (_injector != null)
            {
                _injector.Inject(command, InjectionMode.ResolveOnly);
            }
        }
    }
}