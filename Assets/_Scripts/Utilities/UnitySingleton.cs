using UnityEngine;
using System.Collections;

using System;
using UnityEngine;

/// <summary>
/// Make sure we always have one and only one instance of this class when we need it.
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = SpawnNew();
            }
            return instance;
        }
        protected set
        {
            instance = value;
        }
    }

    private static T SpawnNew()
    {
        GameObject g = new GameObject();
        g.name = "Singleton_ofType_" + typeof(T).ToString();
        return g.AddComponent<T>();
    }

    protected void Awake()
    {
        if (instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
        }
    }
}
