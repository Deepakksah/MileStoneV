using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VideoOS.Common.Integration;

public static class TypeFilteringSerializationBinderUtility
{
	public sealed class TypeFilteringBinder : SerializationBinder
	{
		private SerializationBinder WrappedBinder { get; }

		public Type[] AllowedTypes { get; }

		public TypeFilteringBinder(SerializationBinder wrappedBinder, params Type[] allowedTypes)
		{
			WrappedBinder = wrappedBinder;
			AllowedTypes = allowedTypes;
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			if (!AllowedTypes.Any((Type t) => t.Assembly.FullName.Equals(assemblyName, StringComparison.Ordinal) && t.FullName.Equals(typeName, StringComparison.Ordinal)))
			{
				throw new InvalidOperationException("Deserialization of unknown type " + typeName + " attempted");
			}
			return WrappedBinder?.BindToType(assemblyName, typeName);
		}
	}

	public static BinaryFormatter Wrap(this BinaryFormatter formatter, params Type[] allowedTypes)
	{
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		formatter.Binder = new TypeFilteringBinder(formatter.Binder, allowedTypes);
		return formatter;
	}
}
