using Gonity.Core;
using System;
using System.Collections.Generic;

namespace Gonity
{
    public class Sequence : ISequence
    {
        private IEventDispatcher _eventDispatcher;
        private List<Step> _steps = new List<Step>();
        private int _index;

        public Sequence(IEventDispatcher eventDispatcher = null)
        {
            _eventDispatcher = eventDispatcher;
        }

        public ISequence Call(Callback callback)
        {
            Step step = new CallStep { callback = callback, onStepComplete = OnStepComplete };
            _steps.Add(step);
            return this;
        }

        public ISequence Call<T>(Callback<T> callback, T param)
        {
            Step step = new CallStep<T> { callback = callback, param = param, onStepComplete = OnStepComplete };
            _steps.Add(step);
            return this;
        }

        public ISequence Call<T, U>(Callback<T, U> callback, T param1, U param2)
        {
            Step step = new CallStep<T, U> { callback = callback, param1 = param1, param2 = param2, onStepComplete = OnStepComplete };
            _steps.Add(step);
            return this;
        }

        public ISequence Call<T, U, V>(Callback<T, U, V> callback, T param1, U param2, V param3)
        {
            Step step = new CallStep<T, U, V> { callback = callback, param1 = param1, param2 = param2, param3 = param3, onStepComplete = OnStepComplete };
            _steps.Add(step);
            return this;
        }

        public ISequence WaitFor(Enum eventId, Callback callback = null)
        {
            WaitForStep step = new WaitForStep();
            step.Add(new WaitForContainer { eventDispatcher = _eventDispatcher, eventId = eventId, sequencecallback = OnWaitStepComplete, callback = callback });
            _steps.Add(step);
            return this;
        }

        public ISequence WaitFor<T>(Enum eventId, Callback<T> callback = null)
        {
            WaitForStep step = new WaitForStep();
            step.Add(new WaitForContainer<T> { eventDispatcher = _eventDispatcher, eventId = eventId, sequencecallback = OnWaitStepComplete, callback = callback });
            _steps.Add(step);
            return this;
        }

        public ISequence WaitFor<T, U>(Enum eventId, Callback<T, U> callback = null)
        {
            WaitForStep step = new WaitForStep();
            step.Add(new WaitForContainer<T, U> { eventDispatcher = _eventDispatcher, eventId = eventId, sequencecallback = OnWaitStepComplete, callback = callback });
            _steps.Add(step);
            return this;
        }

        public ISequence WaitFor<T, U, V>(Enum eventId, Callback<T, U, V> callback = null)
        {
            WaitForStep step = new WaitForStep();
            step.Add(new WaitForContainer<T, U, V> { eventDispatcher = _eventDispatcher, eventId = eventId, sequencecallback = OnWaitStepComplete, callback = callback });
            _steps.Add(step);
            return this;
        }

        public void Start()
        {
            _index = 0;
            if (_steps.Count > 0)
            {
                NextStep();
            }
        }

        private void NextStep()
        {
            Step step = _steps[_index];

            if (IsNextStepAsync())
            {
                Step nextStep = _steps[_index + 1];
                nextStep.Run();
            }

            step.Run();
        }

        private bool IsNextStepAsync()
        {
            if (_index + 1 < _steps.Count)
            {
                Step nextStep = _steps[_index + 1];
                return nextStep.IsAsync();
            }

            return false;
        }

        private void OnWaitStepComplete()
        {
            (_steps[_index] as WaitForStep).Clear();
            OnStepComplete();
        }

        private void OnWaitStepComplete<T>(T param)
        {
            (_steps[_index] as WaitForStep).Clear();
            OnStepComplete();
        }

        private void OnWaitStepComplete<T, U>(T param1, U param2)
        {
            (_steps[_index] as WaitForStep).Clear();
            OnStepComplete();
        }

        private void OnWaitStepComplete<T, U, V>(T param1, U param2, V param3)
        {
            (_steps[_index] as WaitForStep).Clear();
            OnStepComplete();
        }

        private void OnStepComplete()
        {
            ++_index;

            if (_index < _steps.Count)
            {
                if (!_steps[_index].IsAsync())
                {
                    NextStep();
                }
            }
        }

        private abstract class Step
        {
            public Callback onStepComplete;

            virtual public bool IsAsync() { return false; }
            public abstract void Run();
        }

        private class CallStep : Step
        {
            public Callback callback;
            public override void Run()
            {
                callback();
                onStepComplete();
            }
        }

        private class CallStep<T> : Step
        {
            public Callback<T> callback;
            public T param;
            public override void Run()
            {
                callback(param);
                onStepComplete();
            }
        }

        private class CallStep<T, U> : Step
        {
            public Callback<T, U> callback;
            public T param1;
            public U param2;
            public override void Run()
            {
                callback(param1, param2);
                onStepComplete();
            }
        }

        private class CallStep<T, U, V> : Step
        {
            public Callback<T, U, V> callback;
            public T param1;
            public U param2;
            public V param3;
            public override void Run()
            {
                callback(param1, param2, param3);
                onStepComplete();
            }
        }

        private abstract class BaseWaitForContainer
        {
            public Enum eventId;
            public IEventDispatcher eventDispatcher;
            abstract public void Clear();
            abstract public void Run();
        }

        private class WaitForContainer : BaseWaitForContainer
        {
            public Callback sequencecallback;
            public Callback callback;

            public override void Run()
            {
                if (callback != null) eventDispatcher.Add(eventId, callback);
                eventDispatcher.Add(eventId, sequencecallback);
            }

            override public void Clear()
            {
                if (callback != null) eventDispatcher.Remove(eventId, callback);
                eventDispatcher.Remove(eventId, sequencecallback);
            }
        }

        private class WaitForContainer<T> : BaseWaitForContainer
        {
            public Callback<T> sequencecallback;
            public Callback<T> callback;

            public override void Run()
            {
                if (callback != null) eventDispatcher.Add(eventId, callback);
                eventDispatcher.Add(eventId, sequencecallback);
            }

            override public void Clear()
            {
                if (callback != null) eventDispatcher.Remove(eventId, callback);
                eventDispatcher.Remove(eventId, sequencecallback);
            }
        }

        private class WaitForContainer<T, U> : BaseWaitForContainer
        {
            public Callback<T, U> sequencecallback;
            public Callback<T, U> callback;

            public override void Run()
            {
                if (callback != null) eventDispatcher.Add(eventId, callback);
                eventDispatcher.Add(eventId, sequencecallback);
            }

            override public void Clear()
            {
                if (callback != null) eventDispatcher.Remove(eventId, callback);
                eventDispatcher.Remove(eventId, sequencecallback);
            }
        }

        private class WaitForContainer<T, U, V> : BaseWaitForContainer
        {
            public Callback<T, U, V> sequencecallback;
            public Callback<T, U, V> callback;

            public override void Run()
            {
                if (callback != null) eventDispatcher.Add(eventId, callback);
                eventDispatcher.Add(eventId, sequencecallback);
            }

            override public void Clear()
            {
                if (callback != null) eventDispatcher.Remove(eventId, callback);
                eventDispatcher.Remove(eventId, sequencecallback);
            }
        }

        private class WaitForStep : Step
        {
            private List<BaseWaitForContainer> _waitForContainers = new List<BaseWaitForContainer>();

            override public bool IsAsync() { return true; }
            public override void Run()
            {
                int l = _waitForContainers.Count;
                for (int i = 0; i < l; ++i)
                {
                    _waitForContainers[i].Run();
                }
            }

            public void Clear()
            {
                int l = _waitForContainers.Count;
                for (int i = 0; i < l; ++i)
                {
                    _waitForContainers[i].Clear();
                }

                _waitForContainers.Clear();
            }

            public void Add(BaseWaitForContainer waitForContainer)
            {
                _waitForContainers.Add(waitForContainer);
            }
        }
    }
}
