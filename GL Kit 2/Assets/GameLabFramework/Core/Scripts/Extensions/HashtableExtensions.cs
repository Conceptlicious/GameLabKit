using UnityEngine;
using System;
using System.Collections;

public static class HashtableExtensions
{
	public static void Set(this Hashtable hashtable, object key, object value)
	{
		if(hashtable.ContainsKey(key)) hashtable[key] = value;
		else hashtable.Add (key, value);
	}
	
	public static void SetIf(this Hashtable hashtable, object key, object value, bool canSet)
	{
		if (canSet) Set (hashtable, key, value);
		else hashtable.SafelyRemove(key.ToString());
	}
	
	public static string Get(this Hashtable hashtable, string key)
	{
		return hashtable.ContainsKey(key) ? hashtable[key].ToString() : string.Empty;
	}
	
	public static Hashtable GetHashtable(this Hashtable hashtable, string key)
	{
		return hashtable.ContainsKey(key) ? hashtable[key].ToHashtable() : new Hashtable();
	}
	
	public static ArrayList GetArrayList(this Hashtable hashtable, string key)
	{
		return hashtable.ContainsKey(key) ? hashtable[key].ToArrayList() : new ArrayList();
	}
	
	public static void SafelyRemove(this Hashtable hashtable, string key)
	{
		if (hashtable.ContainsKey(key))	hashtable.Remove(key);
	}
	
	public static bool IsFilled(this Hashtable hashtable)
	{
		return !IsEmpty(hashtable);
	}
	
	public static bool IsEmpty(this Hashtable hashtable)
	{
		return hashtable != null ? hashtable.Count == 0 : true;
	}
}