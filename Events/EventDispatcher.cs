using Gonity.Core;
using System;
using System.Collections.Generic;

namespace Gonity
{
    public class EventDispatcher : IEventDispatcher
    {
        private Dictionary<Enum, List<BaseCallbackContainer>> _eventCallbacks = new Dictionary<Enum, List<BaseCallbackContainer>>();
        private Dictionary<Enum, List<BaseCallbackContainer>> _callbacks = new Dictionary<Enum, List<BaseCallbackContainer>>();

        private Dictionary<Enum, List<BaseCallbackContainer>> _invalidCallbacks = new Dictionary<Enum, List<BaseCallbackContainer>>();
        private int _dispatchCount = 0;

        public void Add(Enum eventId, Callback callback)
        {
            InternalAdd(eventId, new CallbackContainer { callback = callback }, _callbacks);
        }

        public void Add<T>(Enum eventId, Callback<T> callback)
        {
            InternalAdd(eventId, new CallbackContainer<T> { callback = callback }, _callbacks);
        }

        public void Add<T, U>(Enum eventId, Callback<T, U> callback)
        {
            InternalAdd(eventId, new CallbackContainer<T, U> { callback = callback }, _callbacks);
        }

        public void Add<T, U, V>(Enum eventId, Callback<T, U, V> callback)
        {
            InternalAdd(eventId, new CallbackContainer<T, U, V> { callback = callback }, _callbacks);
        }

        public void Add(Enum eventId, EventCallback callback)
        {
            InternalAdd(eventId, new EventCallbackContainer { callback = callback }, _eventCallbacks);
        }

        public void Add<T>(Enum eventId, EventCallback<T> callback)
        {
            InternalAdd(eventId, new EventCallbackContainer<T> { callback = callback }, _eventCallbacks);
        }

        public void Add<T, U>(Enum eventId, EventCallback<T, U> callback)
        {
            InternalAdd(eventId, new EventCallbackContainer<T, U> { callback = callback }, _eventCallbacks);
        }

        public void Add<T, U, V>(Enum eventId, EventCallback<T, U, V> callback)
        {
            InternalAdd(eventId, new EventCallbackContainer<T, U, V> { callback = callback }, _eventCallbacks);
        }

        public void Remove(Enum eventId, Callback callback)
        {
            if (!_callbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _callbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((CallbackContainer)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _callbacks);
                    break;
                }
            }
        }

        public void Remove<T>(Enum eventId, Callback<T> callback)
        {
            if (!_callbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _callbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((CallbackContainer<T>)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _callbacks);
                    break;
                }
            }
        }

        public void Remove<T, U>(Enum eventId, Callback<T, U> callback)
        {
            if (!_callbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _callbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((CallbackContainer<T, U>)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _callbacks);
                    break;
                }
            }
        }

        public void Remove<T, U, V>(Enum eventId, Callback<T, U, V> callback)
        {
            if (!_callbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _callbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((CallbackContainer<T, U, V>)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _callbacks);
                    break;
                }
            }
        }

        public void Remove(Enum eventId, EventCallback callback)
        {
            if (!_eventCallbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((EventCallbackContainer)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _eventCallbacks);
                    break;
                }
            }
        }

        public void Remove<T>(Enum eventId, EventCallback<T> callback)
        {
            if (!_eventCallbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((EventCallbackContainer<T>)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _eventCallbacks);
                    break;
                }
            }
        }

        public void Remove<T, U>(Enum eventId, EventCallback<T, U> callback)
        {
            if (!_eventCallbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((EventCallbackContainer<T, U>)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _eventCallbacks);
                    break;
                }
            }
        }

        public void Remove<T, U, V>(Enum eventId, EventCallback<T, U, V> callback)
        {
            if (!_eventCallbacks.ContainsKey(eventId))
            {
                return;
            }

            List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
            int l = callbacks.Count;
            for (int i = 0; i < l; ++i)
            {
                BaseCallbackContainer callbackContainer = callbacks[i];
                if (((EventCallbackContainer<T, U, V>)callbackContainer).callback == callback)
                {
                    InternalRemove(eventId, callbackContainer, _eventCallbacks);
                    break;
                }
            }
        }

        public void RemoveAll()
        {
            _eventCallbacks.Clear();
            _callbacks.Clear();
        }

        public void Dispatch(Enum eventId)
        {
            if (!_eventCallbacks.ContainsKey(eventId) && !_callbacks.ContainsKey(eventId)) return;

            ++_dispatchCount;

            if (_callbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _callbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((CallbackContainer)callbackContainer).callback();
                    }
                }
            }

            if (_eventCallbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((EventCallbackContainer)callbackContainer).callback(eventId);
                    }
                }
            }

            CompleteDispatch();
        }

        public void Dispatch<T>(Enum eventId, T param)
        {
            if (!_eventCallbacks.ContainsKey(eventId) && !_callbacks.ContainsKey(eventId)) return;

            ++_dispatchCount;

            if (_callbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _callbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((CallbackContainer<T>)callbackContainer).callback(param);
                    }
                }
            }

            if (_eventCallbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((EventCallbackContainer<T>)callbackContainer).callback(eventId, param);
                    }
                }
            }

            CompleteDispatch();
        }

        public void Dispatch<T, U>(Enum eventId, T param1, U param2)
        {
            if (!_eventCallbacks.ContainsKey(eventId) && !_callbacks.ContainsKey(eventId)) return;

            ++_dispatchCount;

            if (_callbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _callbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((CallbackContainer<T, U>)callbackContainer).callback(param1, param2);
                    }
                }
            }

            if (_eventCallbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((EventCallbackContainer<T, U>)callbackContainer).callback(eventId, param1, param2);
                    }
                }
            }

            CompleteDispatch();
        }

        public void Dispatch<T, U, V>(Enum eventId, T param1, U param2, V param3)
        {
            if (!_eventCallbacks.ContainsKey(eventId) && !_callbacks.ContainsKey(eventId)) return;

            ++_dispatchCount;

            if (_callbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _callbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((CallbackContainer<T, U, V>)callbackContainer).callback(param1, param2, param3);
                    }
                }
            }

            if (_eventCallbacks.ContainsKey(eventId))
            {
                List<BaseCallbackContainer> callbacks = _eventCallbacks[eventId];
                int l = callbacks.Count;
                for (int i = 0; i < l; ++i)
                {
                    BaseCallbackContainer callbackContainer = callbacks[i];
                    if (IsValidContainer(eventId, callbackContainer))
                    {
                        ((EventCallbackContainer<T, U, V>)callbackContainer).callback(eventId, param1, param2, param3);
                    }
                }
            }

            CompleteDispatch();
        }

        private void InternalAdd(Enum eventId, BaseCallbackContainer callbackContainer, Dictionary<Enum, List<BaseCallbackContainer>> dict)
        {
            if (dict.ContainsKey(eventId))
            {
                dict[eventId].Add(callbackContainer);
            }
            else
            {
                dict.Add(eventId, new List<BaseCallbackContainer>() { callbackContainer });
            }
        }

        private void InternalRemove(Enum eventId, BaseCallbackContainer callbackContainer, Dictionary<Enum, List<BaseCallbackContainer>> dict)
        {
            // if dispatch are in progress we do not want to remove them immediately, we wait until all dispatch are complete before doing so
            if (_dispatchCount > 0)
            {
                if (_invalidCallbacks.ContainsKey(eventId))
                {
                    _invalidCallbacks[eventId].Add(callbackContainer);
                }
                else
                {
                    _invalidCallbacks.Add(eventId, new List<BaseCallbackContainer>() { callbackContainer });
                }
            }
            else
            {
                dict[eventId].Remove(callbackContainer);

                if (dict[eventId].Count == 0)
                {
                    dict.Remove(eventId);
                }
            }
        }

        private void CompleteDispatch()
        {
            --_dispatchCount;

            if (_dispatchCount < 0)
            {
                throw new Exception("Dispatch count should never go below zero");
            }

            // All dispatch are now complete, we can clear out all events that have been removed
            if (_dispatchCount == 0)
            {
                var enumerator = _invalidCallbacks.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    List<BaseCallbackContainer> invalids = _invalidCallbacks[enumerator.Current.Key];
                    int l = invalids.Count;
                    for (int i = 0; i < l; ++i)
                    {
                        BaseCallbackContainer callbackContainer = invalids[i];
                        if (_callbacks.ContainsKey(enumerator.Current.Key))
                        {
                            _callbacks[enumerator.Current.Key].Remove(callbackContainer);

                            if (_callbacks[enumerator.Current.Key].Count == 0)
                            {
                                _callbacks.Remove(enumerator.Current.Key);
                            }
                        }

                        if (_eventCallbacks.ContainsKey(enumerator.Current.Key))
                        {
                            _eventCallbacks[enumerator.Current.Key].Remove(callbackContainer);

                            if (_eventCallbacks[enumerator.Current.Key].Count == 0)
                            {
                                _eventCallbacks.Remove(enumerator.Current.Key);
                            }
                        }
                    }

                }

                _invalidCallbacks.Clear();
            }
        }

        private bool IsValidContainer(Enum eventId, BaseCallbackContainer callbackContainer)
        {
            if (_invalidCallbacks.ContainsKey(eventId) && _invalidCallbacks[eventId].Find(cb => callbackContainer == cb) != null)
            {
                return false;
            }

            return true;
        }

        private abstract class BaseCallbackContainer { }

        private class CallbackContainer : BaseCallbackContainer
        {
            public Callback callback;
        }

        private class CallbackContainer<T> : BaseCallbackContainer
        {
            public Callback<T> callback;
        }

        private class CallbackContainer<T, U> : BaseCallbackContainer
        {
            public Callback<T, U> callback;
        }

        private class CallbackContainer<T, U, V> : BaseCallbackContainer
        {
            public Callback<T, U, V> callback;
        }

        private class EventCallbackContainer : BaseCallbackContainer
        {
            public EventCallback callback;
        }

        private class EventCallbackContainer<T> : BaseCallbackContainer
        {
            public EventCallback<T> callback;
        }

        private class EventCallbackContainer<T, U> : BaseCallbackContainer
        {
            public EventCallback<T, U> callback;
        }

        private class EventCallbackContainer<T, U, V> : BaseCallbackContainer
        {
            public EventCallback<T, U, V> callback;
        }
    }
}