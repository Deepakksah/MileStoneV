using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DataProperty", Namespace = "http://videoos.net/2/GisMapCommand")]
[KnownType("GetKnownTypes")]
public class DataProperty : IExtensibleDataObject
{
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string DataType { get; set; }

	[DataMember]
	public object Value { get; set; }

	[DataMember]
	public DataProperty[] NestedProperties { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	private static IEnumerable<Type> GetKnownTypes()
	{
		return new List<Type>
		{
			typeof(EmptyDataPropertyArray),
			typeof(NullDataPropertyArray),
			typeof(bool[]),
			typeof(byte[]),
			typeof(char[]),
			typeof(decimal[]),
			typeof(double[]),
			typeof(float[]),
			typeof(int[]),
			typeof(long[]),
			typeof(sbyte[]),
			typeof(short[]),
			typeof(uint[]),
			typeof(ulong[]),
			typeof(ushort[]),
			typeof(string[]),
			typeof(Guid[]),
			typeof(DateTime[]),
			typeof(TimeSpan[])
		};
	}
}
