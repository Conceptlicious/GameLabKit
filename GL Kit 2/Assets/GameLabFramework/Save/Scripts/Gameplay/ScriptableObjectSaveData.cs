using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLab
{
	public struct ScriptableObjectSaveData<T> where T : ScriptableObject
	{
		public T ScriptableObject;

		public ScriptableObjectSaveData(T scriptableObject)
		{
			ScriptableObject = scriptableObject;
		}
	}
}
