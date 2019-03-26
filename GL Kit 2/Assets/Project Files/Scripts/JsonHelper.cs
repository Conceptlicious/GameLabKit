using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper 
{
    public static T[] FromJson<T>(string jsonString)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(jsonString);
        return wrapper.rooms;
    }
    
    [Serializable]
    private class Wrapper<T>
    {
        public T[] rooms;
    }
}
