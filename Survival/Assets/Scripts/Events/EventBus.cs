using System;
using UnityEngine;

public static class EventBus<T>
{
    private static event Action<T> OnEvent;
    public static void Subscribe(Action<T> action) => OnEvent += action;
    public static void Unsubscribe(Action<T> action) => OnEvent -= action;
    public static void Raise(T evt) => OnEvent?.Invoke(evt);
}
