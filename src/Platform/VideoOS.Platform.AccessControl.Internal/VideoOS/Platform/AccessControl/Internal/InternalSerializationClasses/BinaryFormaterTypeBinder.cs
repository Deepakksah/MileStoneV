using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

public sealed class BinaryFormaterTypeBinder : SerializationBinder
{
	private SerializationBinder WrappedBinder { get; }

	private IEnumerable<Type> AllowedTypes { get; }

	public BinaryFormaterTypeBinder(SerializationBinder wrappedBinder, IEnumerable<Type> allowedTypes)
	{
		WrappedBinder = wrappedBinder;
		AllowedTypes = allowedTypes;
	}

	public override Type BindToType(string assemblyName, string typeName)
	{
		if (AllowedTypes.FirstOrDefault((Type t) => t.Assembly.FullName.Equals(assemblyName, StringComparison.Ordinal) && t.FullName.Equals(typeName, StringComparison.Ordinal)) == null)
		{
			if (assemblyName.Equals("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", StringComparison.OrdinalIgnoreCase))
			{
				return WrappedBinder?.BindToType(assemblyName, typeName);
			}
			throw new Exception("Disallowed type deserialization attempt! Type: " + typeName);
		}
		return WrappedBinder?.BindToType(assemblyName, typeName);
	}
}
