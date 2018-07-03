using System;
using System.Collections.Generic;

namespace Gonity
{
    public class StateMachine
    {
        StateItem m_currentState;
        Enum m_currentStateID;

        Dictionary<Enum, StateItem> _states = new Dictionary<Enum, StateItem>();

        private struct StateItem
        {
            public Action enterAction;
            public Action updateAction;
            public Action lateUpdateAction;
            public Action exitAction;
        }

        public Enum currentState
        {
            get { return m_currentStateID; }
        }

        public void RegisterState(Enum state, Action enterAction = null, Action updateAction = null, Action exitAction = null, Action lateUpdateAction = null)
        {
            StateItem stateItem = new StateItem();
            stateItem.enterAction = enterAction;
            stateItem.updateAction = updateAction;
            stateItem.lateUpdateAction = lateUpdateAction;
            stateItem.exitAction = exitAction;

            _states.Add(state, stateItem);
        }

        public void ChangeState(Enum state)
        {
            if (m_currentState.exitAction != null)
            {
                m_currentState.exitAction();
            }

            m_currentStateID = state;
            m_currentState = _states[state];

            if (m_currentState.enterAction != null)
            {
                m_currentState.enterAction();
            }
        }

        public bool IsCurrentState(Enum state)
        {
            return m_currentStateID.Equals(state);
        }

        public void Update()
        {
            if (m_currentState.updateAction != null)
            {
                m_currentState.updateAction();
            }
        }

        public void LateUpdate()
        {
            if (m_currentState.lateUpdateAction != null)
            {
                m_currentState.lateUpdateAction();
            }
        }

        public static implicit operator bool(StateMachine stateMachine)
        {
            return !ReferenceEquals(stateMachine, null);
        }
    }
}