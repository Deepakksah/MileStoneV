using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class EnumerationContentDefinition
{
	private MainDefinition mainDefinition;

	private CapabilityDefinition capability;

	public enumerationType Base { get; private set; }

	public Collection<EnumerationValue> Values
	{
		get
		{
			Collection<EnumerationValue> collection = new Collection<EnumerationValue>();
			if (Base.value != null)
			{
				enumValueType[] value = Base.value;
				foreach (enumValueType enumValueType2 in value)
				{
					if (TypeDefinition.SelectResult(mainDefinition, capability, enumValueType2.select))
					{
						collection.Add(new EnumerationValue(this, enumValueType2));
					}
				}
			}
			return collection;
		}
	}

	public enumValueType[] GetActiveEnumerationValues()
	{
		List<enumValueType> list = new List<enumValueType>();
		if (Base.value != null)
		{
			enumValueType[] value = Base.value;
			foreach (enumValueType enumValueType2 in value)
			{
				if (TypeDefinition.SelectResult(mainDefinition, capability, enumValueType2.select))
				{
					list.Add(enumValueType2);
				}
			}
		}
		return list.ToArray();
	}

	public EnumerationContentDefinition(MainDefinition mainDefinition, CapabilityDefinition capability, enumerationType enumerationType)
	{
		this.mainDefinition = mainDefinition;
		this.capability = capability;
		Base = enumerationType;
	}

	public static bool HasIdenticalRanges(EnumerationContentDefinition enumerationContentDefinition1, EnumerationContentDefinition enumerationContentDefinition2)
	{
		if (enumerationContentDefinition1 == null)
		{
			throw new ArgumentNullException("enumerationContentDefinition1");
		}
		if (enumerationContentDefinition2 == null)
		{
			throw new ArgumentNullException("enumerationContentDefinition2");
		}
		enumValueType[] activeEnumerationValues = enumerationContentDefinition1.GetActiveEnumerationValues();
		enumValueType[] activeEnumerationValues2 = enumerationContentDefinition2.GetActiveEnumerationValues();
		if (activeEnumerationValues.Length != activeEnumerationValues2.Length)
		{
			return false;
		}
		enumValueType[] array = activeEnumerationValues;
		foreach (enumValueType enumValueType2 in array)
		{
			bool flag = false;
			enumValueType[] array2 = activeEnumerationValues2;
			foreach (enumValueType enumValueType3 in array2)
			{
				if (string.Equals(enumValueType2.referenceid, enumValueType3.referenceid, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}
}
