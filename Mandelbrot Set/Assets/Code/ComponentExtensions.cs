using System;
using UnityEngine;

public static class ComponentExtensions
{
    public static T EnsureComponent<T>(this Component component) where T : Component
    {
        if (!component.TryGetComponent(out T result))
        {
            result = component.gameObject.AddComponent<T>();
        }

        return result;
    }
}
