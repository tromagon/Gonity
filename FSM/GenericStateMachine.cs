using System;
using System.Collections.Generic;

namespace Gonity
{
    public class GenericStateMachine<T>
    {
        private struct StateItem
        {
            public Enum stateId;
            public Action<T> enterAction;
            public Action<T> updateAction;
            public Action<T> lateUpdateAction;
            public Action<T> exitAction;
        }

        private T _owner;
        private StateItem _currentState;
        private Dictionary<Enum, StateItem> _states = new Dictionary<Enum, StateItem>();

        public GenericStateMachine(T owner)
        {
            _owner = owner;
        }

        public void RegisterState(Enum stateId, Action<T> enterAction = null, Action<T> updateAction = null, Action<T> exitAction = null, Action<T> lateUpdateAction = null)
        {
            StateItem stateItem = new StateItem
            {
                stateId = stateId,
                enterAction = enterAction,
                updateAction = updateAction,
                lateUpdateAction = lateUpdateAction,
                exitAction = exitAction
            };

            _states.Add(stateId, stateItem);
        }

        public void SetState(Enum state)
        {
            if (_currentState.exitAction != null)
            {
                _currentState.exitAction(_owner);
            }

            _currentState = _states[state];

            if (_currentState.enterAction != null)
            {
                _currentState.enterAction(_owner);
            }
        }

        public bool IsCurrentState(Enum stateId)
        {
            return _currentState.stateId.Equals(stateId);
        }

        public void Update()
        {
            if (_currentState.updateAction != null)
            {
                _currentState.updateAction(_owner);
            }
        }

        public void LateUpdate()
        {
            if (_currentState.lateUpdateAction != null)
            {
                _currentState.lateUpdateAction(_owner);
            }
        }

        public static implicit operator bool(GenericStateMachine<T> stateMachine)
        {
            return !ReferenceEquals(stateMachine, null);
        }
    }
}