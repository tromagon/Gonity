using System.Collections.Generic;
using System;

namespace Gonity
{
    public class SystemMap : ISystemMap, IUpdate, ILateUpdate
    {
        private IEntityDatabase _entityDatabase;
        public List<IUpdate> _updates;
        public List<ILateUpdate> _lateUpdates;

        public SystemMap(IEntityDatabase entityDatabase)
        {
            _entityDatabase = entityDatabase;
            _updates = new List<IUpdate>();
            _lateUpdates = new List<ILateUpdate>();
        }

        public void Add(ECSSystem system)
        {
            bool updateFound = false;

            system.Init(_entityDatabase);
            if (system is IUpdate)
            {
                updateFound = true;
                _updates.Add(system as IUpdate);
            }

            if (system is ILateUpdate)
            {
                updateFound = true;
                _lateUpdates.Add(system as ILateUpdate);
            }

            if (!updateFound) throw new Exception(string.Format("{0} needs to implement IUpdate or ILateUpdate", system));
        }

        public void Update()
        {
            for (int i = 0; i < _updates.Count; ++i)
            {
                _updates[i].Update();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < _lateUpdates.Count; ++i)
            {
                _lateUpdates[i].LateUpdate();
            }
        }
    }
}
