using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    private static readonly object go = new object();

    public static T Instance
    {
        get
        {
            lock (go)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError(
                            string.Format(
                                "class {0} has more then one singleton gameobject instance in the scene", typeof(T)));
                        return instance;
                    }

                    if (instance == null)
                    {
                        var singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton)" + typeof(T);
                        singleton.hideFlags = HideFlags.None;
                    }
                }
                instance.hideFlags = HideFlags.None;
                return instance;
            }
        }
    }
}