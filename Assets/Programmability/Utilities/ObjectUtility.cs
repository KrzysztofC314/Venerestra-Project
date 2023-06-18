using UnityEngine;

public static class ObjectUtility
{
    public static bool IsDestroyed(this Object obj)
    {
        return obj == null && !ReferenceEquals(obj, null);
    }
}