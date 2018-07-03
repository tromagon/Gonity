using System;
using System.Collections.Generic;

namespace Gonity
{
    public class Updater : IUpdater, IUpdate, ILateUpdate
    {
        private List<Action> _updates;
        private List<Action> _lateUpdates;

        public Updater()
        {
            _updates = new List<Action>();
            _lateUpdates = new List<Action>();
        }

        public void RunUpdate(Action action)
        {
            _updates.Add(action);
        }

        public void RunLateUpdate(Action action)
        {
            _lateUpdates.Add(action);
        }

        public void StopUpdate(Action action)
        {
            _updates.Remove(action);
        }

        public void StopLateUpdate(Action action)
        {
            _lateUpdates.Remove(action);
        }

        public void Update()
        {
            for (int i = 0; i < _updates.Count; ++i)
            {
                _updates[i]();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < _lateUpdates.Count; ++i)
            {
                _lateUpdates[i]();
            }
        }
    }
}
