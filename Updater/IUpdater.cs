using System;

namespace Gonity
{
    public interface IUpdater
    {
        void RunUpdate(Action action);
        void RunLateUpdate(Action action);
        void StopUpdate(Action action);
        void StopLateUpdate(Action action);
    }
}