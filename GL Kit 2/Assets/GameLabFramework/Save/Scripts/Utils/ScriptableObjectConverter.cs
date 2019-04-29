using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameLab
{
	public class ScriptableObjectConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType)
		{
			return typeof(ScriptableObject).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			ScriptableObject obj = ScriptableObject.CreateInstance(objectType);
			serializer.Populate(reader, obj);
			return obj;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
