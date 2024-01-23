using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonobehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
            Destroy(Instance);
    }
}
