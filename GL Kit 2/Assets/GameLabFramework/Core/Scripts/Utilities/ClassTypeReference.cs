// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using UnityEngine;

namespace GameLab
{

	/// <summary>
	/// Reference to a class <see cref="System.Type"/> with support for Unity serialization.
	/// </summary>
	[Serializable]
	public sealed class ClassTypeReference : ISerializationCallbackReceiver
	{
		// https://bitbucket.org/kevinjfields/classtypereference-for-unity/overview
		private Type type;

		/// <summary>
		/// Gets or sets type of class reference.
		/// </summary>
		/// <exception cref="System.ArgumentException">
		/// If <paramref name="value"/> is not a class type.
		/// </exception>
		public Type Type
		{
			get => type;
			set
			{
				if(value != null && !value.IsClass)
					throw new ArgumentException(string.Format("'{0}' is not a class type.", value.FullName), "value");

				type = value;
				classRef = GetClassRef(value);
			}
		}

		[SerializeField] private string classRef;

		public static string GetClassRef(Type type) => type == null ? string.Empty : $"{type.FullName}, {type.Assembly.GetName().Name}";

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassTypeReference"/> class.
		/// </summary>
		public ClassTypeReference() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassTypeReference"/> class.
		/// </summary>
		/// <param name="type">Class type.</param>
		/// <exception cref="System.ArgumentException">
		/// If <paramref name="type"/> is not a class type.
		/// </exception>
		public ClassTypeReference(Type type)
		{
			Type = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassTypeReference"/> class.
		/// </summary>
		/// <param name="assemblyQualifiedClassName">Assembly qualified class name</param>
		/// <exception cref="System.ArgumentException">
		/// If <paramref name="assemblyQualifiedClassName"/> is null or empty.
		/// </exception>
		public ClassTypeReference(string assemblyQualifiedClassName)
		{
			if(string.IsNullOrEmpty(assemblyQualifiedClassName))
			{
				throw new ArgumentException($"'{assemblyQualifiedClassName}' is not a valid assembly qualified class name.");
			}

			Type = Type.GetType(assemblyQualifiedClassName);
		}

		public void OnBeforeSerialize() { }
		public void OnAfterDeserialize()
		{
			type = null;

			if(string.IsNullOrEmpty(classRef))
			{
				return;
			}

			type = Type.GetType(classRef);

			if(type == null)
			{
				Debug.LogWarning(string.Format("'{0}' was referenced but class type was not found.", classRef));
			}
		}

		public override string ToString() => Type != null ? Type.FullName : "(None)";

		public static implicit operator string(ClassTypeReference typeReference) => typeReference.classRef;

		public static implicit operator Type(ClassTypeReference typeReference) => typeReference.Type;

		public static implicit operator ClassTypeReference(Type type) => new ClassTypeReference(type);
	}

}
