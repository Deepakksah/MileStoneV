using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class CapabilityDefinition
{
	private MainDefinition mainDefinition;

	private capabilitiesType capabilities;

	private capabilityType capability;

	private TypeDefinition typeDefinition;

	public capabilityType Base
	{
		[DebuggerStepThrough]
		get
		{
			return capability;
		}
	}

	public string FullName => string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", capabilities.hwid, capability.name);

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return capability.name;
		}
	}

	public Hwid Hwid
	{
		[DebuggerStepThrough]
		get
		{
			return Hwid.ParseHwid(capabilities.hwid);
		}
	}

	public bool ResetFeedRequired
	{
		[DebuggerStepThrough]
		get
		{
			if (!capability.resetfeedSpecified)
			{
				return false;
			}
			return capability.resetfeed;
		}
	}

	public int ResetWaitMilliseconds
	{
		[DebuggerStepThrough]
		get
		{
			if (int.TryParse(capability.resetwaitms, NumberStyles.None, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return 1000;
		}
	}

	public bool IncludeInInitialize
	{
		[DebuggerStepThrough]
		get
		{
			if (!capability.initsetSpecified)
			{
				return false;
			}
			return capability.initset;
		}
	}

	public TypeDefinition TypeDefinition
	{
		get
		{
			if (typeDefinition != null)
			{
				return typeDefinition;
			}
			lock (this)
			{
				if (typeDefinition != null)
				{
					return typeDefinition;
				}
				typeType typeType2 = mainDefinition.Base.FindTypeByName(capability.type);
				if (typeType2 == null)
				{
					throw new InvalidDefinitionException(string.Format(CultureInfo.InvariantCulture, "Type not found in definition: {0}", capability.type ?? ""));
				}
				typeDefinition = new TypeDefinition(mainDefinition, this, typeType2);
			}
			return typeDefinition;
		}
	}

	public CapabilityDefinition(MainDefinition mainDefinition, capabilitiesType capabilities, capabilityType capability)
	{
		this.mainDefinition = mainDefinition;
		this.capabilities = capabilities;
		this.capability = capability;
		typeDefinition = null;
	}

	public bool ValidateValue(object value, IFormatProvider provider)
	{
		TypeContent activeTypeContent = TypeDefinition.GetActiveTypeContent();
		if (activeTypeContent != null)
		{
			return TypeDefinition.ActiveTypeName switch
			{
				typename.enumeration => ValidateEnumerationValue(activeTypeContent, value, provider), 
				typename.data => ValidateDataContentValue(activeTypeContent, value, provider), 
				_ => false, 
			};
		}
		return true;
	}

	private static bool ValidateEnumerationValue(TypeContent typecontent, object value, IFormatProvider provider)
	{
		if (typecontent == null || !typecontent.IsEnumeration)
		{
			return false;
		}
		object obj = DefinitionUtility.DecodeTypedValue(datatype.@string, value, provider);
		enumValueType[] activeEnumerationValues = typecontent.AsEnumeration.GetActiveEnumerationValues();
		foreach (enumValueType enumValueType2 in activeEnumerationValues)
		{
			object obj2 = DefinitionUtility.DecodeTypedValue(datatype.@string, enumValueType2.name, CultureInfo.InvariantCulture);
			if (obj.Equals(obj2))
			{
				return true;
			}
		}
		return false;
	}

	private bool ValidateDataContentValue(TypeContent typecontent, object value, IFormatProvider provider)
	{
		if (typecontent != null && typecontent.IsData)
		{
			switch (TypeDefinition.ActiveDataType)
			{
			case datatype.@int:
				return ValidateIntValue(typecontent, value, provider);
			case datatype.@double:
				return ValidateDoubleValue(typecontent, value, provider);
			}
		}
		return true;
	}

	private static bool ValidateIntValue(TypeContent typecontent, object value, IFormatProvider provider)
	{
		int num;
		try
		{
			num = Convert.ToInt32(value, provider);
		}
		catch (FormatException innerException)
		{
			throw new ArgumentException("Unable to convert the value to an Int32.", "value", innerException);
		}
		catch (InvalidCastException innerException2)
		{
			throw new ArgumentException("Unable to convert the value to an Int32.", "value", innerException2);
		}
		catch (OverflowException innerException3)
		{
			throw new ArgumentException("Unable to convert the value to an Int32.", "value", innerException3);
		}
		try
		{
			if (typecontent != null && typecontent.AsData.Base.minvalue != null)
			{
				int num2 = Convert.ToInt32(typecontent.AsData.Base.minvalue, CultureInfo.InvariantCulture);
				if (num < num2)
				{
					return false;
				}
			}
			if (typecontent != null && typecontent.AsData.Base.maxvalue != null)
			{
				int num3 = Convert.ToInt32(typecontent.AsData.Base.maxvalue, CultureInfo.InvariantCulture);
				if (num > num3)
				{
					return false;
				}
			}
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new InvalidDefinitionException("The value could not be validated due to errors in the driver definition.", inner);
		}
	}

	private static bool ValidateDoubleValue(TypeContent typecontent, object value, IFormatProvider provider)
	{
		double num;
		try
		{
			num = Convert.ToDouble(value, provider);
		}
		catch (FormatException innerException)
		{
			throw new ArgumentException("Unable to convert the value to a double.", "value", innerException);
		}
		catch (InvalidCastException innerException2)
		{
			throw new ArgumentException("Unable to convert the value to a double.", "value", innerException2);
		}
		catch (OverflowException innerException3)
		{
			throw new ArgumentException("Unable to convert the value to a double.", "value", innerException3);
		}
		try
		{
			if (typecontent != null && typecontent.AsData.Base.minvalue != null)
			{
				double num2 = Convert.ToDouble(typecontent.AsData.Base.minvalue, CultureInfo.InvariantCulture);
				if (num < num2)
				{
					return false;
				}
			}
			if (typecontent != null && typecontent.AsData.Base.maxvalue != null)
			{
				double num3 = Convert.ToDouble(typecontent.AsData.Base.maxvalue, CultureInfo.InvariantCulture);
				if (num > num3)
				{
					return false;
				}
			}
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new InvalidDefinitionException("The value could not be validated due to errors in the driver definition.", inner);
		}
	}
}
