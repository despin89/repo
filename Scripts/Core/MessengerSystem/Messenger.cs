namespace GD.Core.MessengerSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Game.ActionsSystem;
    using Game.Combat;
    using Game.StatusesSystem;

    internal static class MessengerInternal
    {
        #region Constants

        public static readonly Dictionary<string, Delegate> EventTable = new Dictionary<string, Delegate>();

        #endregion

        #region Вложенный тип: BroadcastException

        public class BroadcastException : Exception
        {
            #region Constructors

            public BroadcastException(string msg)
                : base(msg)
            {
            }

            #endregion
        }

        #endregion

        #region Вложенный тип: ListenerException

        public class ListenerException : Exception
        {
            #region Constructors

            public ListenerException(string msg)
                : base(msg)
            {
            }

            #endregion
        }

        #endregion

        #region Публичные методы

        public static void AddListener(string eventType, Delegate callback)
        {
            OnListenerAdding(eventType, callback);
            EventTable[eventType] = Delegate.Combine(EventTable[eventType], callback);
        }

        public static void RemoveListener(string eventType, Delegate handler)
        {
            OnListenerRemoving(eventType, handler);
            EventTable[eventType] = Delegate.Remove(EventTable[eventType], handler);
            OnListenerRemoved(eventType);
        }

        public static T[] GetInvocationList<T>(string eventType)
        {
            Delegate d;
            if (EventTable.TryGetValue(eventType, out d))
            {
                if (d != null)
                {
                    return d.GetInvocationList().Cast<T>().ToArray();
                }

                throw CreateBroadcastSignatureException(eventType);
            }

            return null;
        }

        public static void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
        {
            if (!EventTable.ContainsKey(eventType))
            {
                EventTable.Add(eventType, null);
            }

            Delegate d = EventTable[eventType];
            if ((d != null) && (d.GetType() != listenerBeingAdded.GetType()))
            {
                throw new ListenerException(
                                            string.Format(
                                                          "Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}",
                                                          eventType,
                                                          d.GetType().Name,
                                                          listenerBeingAdded.GetType().Name));
            }
        }

        public static void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
        {
            if (EventTable.ContainsKey(eventType))
            {
                Delegate d = EventTable[eventType];

                if (d == null)
                {
                    throw new ListenerException(
                                                string.Format(
                                                              "Attempting to remove listener with for event type {0} but current listener is null.",
                                                              eventType));
                }
                if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(
                                                string.Format(
                                                              "Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}",
                                                              eventType,
                                                              d.GetType().Name,
                                                              listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                throw new ListenerException(
                                            string.Format(
                                                          "Attempting to remove listener for type {0} but Messenger doesn't know about this event type.",
                                                          eventType));
            }
        }

        public static void OnListenerRemoved(string eventType)
        {
            if (EventTable[eventType] == null)
            {
                EventTable.Remove(eventType);
            }
        }

        public static void OnBroadcasting(string eventType)
        {
            //Debug.Log("Broadcasting: " + eventName);
        }

        public static BroadcastException CreateBroadcastSignatureException(string eventType)
        {
            return
                new BroadcastException(
                                       string.Format(
                                                     "Broadcasting message {0} but listeners have a different signature than the broadcaster.",
                                                     eventType));
        }

        #endregion
    }

    // No parameters
    public static class Messenger
    {
        #region Публичные методы

        public static void AddListener(string eventType, Action handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void AddListener<TReturn>(string eventType, Func<TReturn> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void RemoveListener(string eventType, Action handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void RemoveListener<TReturn>(string eventType, Func<TReturn> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void Broadcast(string eventType)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Action[] invocationList = MessengerInternal.GetInvocationList<Action>(eventType);

            foreach (Action callback in invocationList)
            {
                callback.Invoke();
            }
        }

        public static void Broadcast<TReturn>(string eventType, Action<TReturn> returnCall)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Func<TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<TReturn>>(eventType);

            foreach (TReturn result in invocationList.Select(del => del.Invoke()))
            {
                returnCall.Invoke(result);
            }
        }

        #endregion
    }

    // One parameter
    public static class Messenger<T>
    {
        #region Публичные методы

        public static void AddListener(string eventType, Action<T> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void AddListener<TReturn>(string eventType, Func<T, TReturn> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void RemoveListener(string eventType, Action<T> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void RemoveListener<TReturn>(string eventType, Func<T, TReturn> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void Broadcast(string eventType, T arg1)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Action<T>[] invocationList = MessengerInternal.GetInvocationList<Action<T>>(eventType);

            if (invocationList == null)
            {
                return;
            }

            foreach (Action<T> callback in invocationList)
            {
                callback.Invoke(arg1);
            }
        }

        public static void Broadcast<TReturn>(string eventType, T arg1, Action<TReturn> returnCall)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Func<T, TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<T, TReturn>>(eventType);

            foreach (TReturn result in invocationList.Select(del => del.Invoke(arg1)).Cast<TReturn>())
            {
                returnCall.Invoke(result);
            }
        }

        #endregion
    }

    // Two parameters
    public static class Messenger<T, U>
    {
        #region Публичные методы

        public static void AddListener(string eventType, Action<T, U> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void AddListener<TReturn>(string eventType, Func<T, U, TReturn> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void RemoveListener(string eventType, Action<T, U> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void RemoveListener<TReturn>(string eventType, Func<T, U, TReturn> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void Broadcast(string eventType, T arg1, U arg2)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Action<T, U>[] invocationList = MessengerInternal.GetInvocationList<Action<T, U>>(eventType);

            if (invocationList == null) return;

            foreach (Action<T, U> callback in invocationList)
            {
                callback.Invoke(arg1, arg2);
            }
        }

        public static void Broadcast<TReturn>(string eventType,
            T arg1,
            U arg2,
            Action<TReturn> returnCall)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Func<T, U, TReturn>[] invocationList = MessengerInternal.GetInvocationList<Func<T, U, TReturn>>(eventType);

            foreach (TReturn result in invocationList.Select(del => del.Invoke(arg1, arg2)).Cast<TReturn>())
            {
                returnCall.Invoke(result);
            }
        }

        #endregion
    }

    // Three parameters
    public static class Messenger<T, U, V>
    {
        #region Публичные методы

        public static void AddListener(string eventType, Action<T, U, V> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void AddListener<TReturn>(string eventType, Func<T, U, V, TReturn> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void RemoveListener(string eventType, Action<T, U, V> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void RemoveListener<TReturn>(string eventType, Func<T, U, V, TReturn> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void Broadcast(string eventType, T arg1, U arg2, V arg3)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Action<T, U, V>[] invocationList = MessengerInternal.GetInvocationList<Action<T, U, V>>(eventType);

            foreach (Action<T, U, V> callback in invocationList)
            {
                callback.Invoke(arg1, arg2, arg3);
            }
        }

        public static void Broadcast<TReturn>(string eventType,
            T arg1,
            U arg2,
            V arg3,
            Action<TReturn> returnCall)
        {
            MessengerInternal.OnBroadcasting(eventType);
            Func<T, U, V, TReturn>[] invocationList =
                MessengerInternal.GetInvocationList<Func<T, U, V, TReturn>>(eventType);

            foreach (TReturn result in invocationList.Select(del => del.Invoke(arg1, arg2, arg3)).Cast<TReturn>())
            {
                returnCall.Invoke(result);
            }
        }

        #endregion
    }

    //Combat event
    public static class CombatMessenger
    {
        #region Публичные методы

        public static void AddListener(string eventType, Action<CombatEventArgs> handler)
        {
            MessengerInternal.AddListener(eventType, handler);
        }

        public static void RemoveListener(string eventType, Action<CombatEventArgs> handler)
        {
            MessengerInternal.RemoveListener(eventType, handler);
        }

        public static void Broadcast(string eventName, CombatEventArgs e)
        {
            MessengerInternal.OnBroadcasting(eventName);
            Action<CombatEventArgs>[] invocationList =
                MessengerInternal.GetInvocationList<Action<CombatEventArgs>>(eventName);

            if (invocationList == null) return;

            foreach (Action<CombatEventArgs> callback in invocationList)
            {
                callback.Invoke(e);
            }
        }

        #endregion
    }
}