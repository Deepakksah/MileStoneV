using System;

namespace VideoOS.GisMapSharedData.DataContracts.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DatagramAttribute : Attribute
{
	public Type DatagramType { get; private set; }

	public DatagramAttribute(Type datagramType)
	{
		if (datagramType == null)
		{
			throw new ArgumentNullException("datagramType");
		}
		DatagramType = datagramType;
	}
}
