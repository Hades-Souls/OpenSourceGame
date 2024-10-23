using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void Execute()
    {
        //Debug.Log("Peersitant object LOad into the scene");
       // Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PersistObject")));
    }

}
