using System;
using System.Collections.Generic;

namespace Infrastructure.Events
{
    /// <summary>
    /// Simple thread-safe static EventBus for publishing and subscribing to typed events.
    /// This implementation is independent from the main game code and has no external dependencies.
    /// 
    /// Usage:
    /// EventBus.Subscribe&lt;PlayerDiedEvent&gt;(OnPlayerDied);
    /// EventBus.Publish(new PlayerDiedEvent { PlayerId = 5 });
    /// EventBus.Unsubscribe&lt;PlayerDiedEvent&gt;(OnPlayerDied);
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();
        private static readonly object _lock = new object();

        /// <summary>
        /// Subscribe to events of type T.
        /// </summary>
        public static void Subscribe<T>(Action<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            lock (_lock)
            {
                var type = typeof(T);
                if (!_subscribers.TryGetValue(type, out var list))
                {
                    list = new List<Delegate>();
                    _subscribers[type] = list;
                }

                // prevent double subscription of the same delegate instance
                if (!list.Contains(handler))
                    list.Add(handler);
            }
        }

        /// <summary>
        /// Unsubscribe from events of type T.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            lock (_lock)
            {
                var type = typeof(T);
                if (!_subscribers.TryGetValue(type, out var list)) return;

                list.Remove(handler);
                if (list.Count == 0)
                    _subscribers.Remove(type);
            }
        }

        /// <summary>
        /// Publish an event to all subscribers of type T.
        /// Exceptions thrown by individual handlers are caught and re-thrown as an AggregateException
        /// after other handlers were invoked.
        /// </summary>
        public static void Publish<T>(T evt)
        {
            List<Delegate> handlersCopy = null;
            lock (_lock)
            {
                if (_subscribers.TryGetValue(typeof(T), out var list))
                {
                    // shallow copy to allow handlers to unsubscribe during invocation
                    handlersCopy = new List<Delegate>(list);
                }
            }

            if (handlersCopy == null || handlersCopy.Count == 0)
                return;

            List<Exception> exceptions = null;
            foreach (var d in handlersCopy)
            {
                try
                {
                    ((Action<T>)d)(evt);
                }
                catch (Exception ex)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(ex);
                }
            }

            if (exceptions != null)
                throw new AggregateException("One or more EventBus handlers threw exceptions.", exceptions);
        }

        /// <summary>
        /// Clears all subscriptions. Useful for tests or scene reload cleanup.
        /// </summary>
        public static void ClearAll()
        {
            lock (_lock)
            {
                _subscribers.Clear();
            }
        }

        /// <summary>
        /// Returns whether there are subscribers for event type T.
        /// </summary>
        public static bool HasSubscribers<T>()
        {
            lock (_lock)
            {
                return _subscribers.TryGetValue(typeof(T), out var list) && list.Count > 0;
            }
        }
    }
}

