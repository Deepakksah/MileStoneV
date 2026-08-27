using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class TypeContent
{
	private MainDefinition mainDefinition;

	private CapabilityDefinition capability;

	private typedefType parentTypeDefitionType;

	private typedefcontentType content;

	public typedefType ParentTypeDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return parentTypeDefitionType;
		}
	}

	public typedefcontentType Base
	{
		[DebuggerStepThrough]
		get
		{
			return content;
		}
	}

	public string DefaultValue
	{
		get
		{
			if (IsData)
			{
				return AsData.Base.defaultvalue;
			}
			if (IsEnumeration)
			{
				return AsEnumeration.Base.defaultvalue;
			}
			return "";
		}
	}

	public bool IsEnumeration
	{
		[DebuggerStepThrough]
		get
		{
			return content is enumerationType;
		}
	}

	public bool IsData
	{
		[DebuggerStepThrough]
		get
		{
			return content is datacontentType;
		}
	}

	public EnumerationContentDefinition AsEnumeration
	{
		get
		{
			if (content is enumerationType)
			{
				return new EnumerationContentDefinition(mainDefinition, capability, (enumerationType)content);
			}
			return null;
		}
	}

	public DataContentDefinition AsData
	{
		get
		{
			if (content is datacontentType)
			{
				return new DataContentDefinition(this, (datacontentType)content);
			}
			return null;
		}
	}

	public TypeContent(MainDefinition mainDefinition, CapabilityDefinition capability, typedefType parentTypeDefinitionType, typedefcontentType content)
	{
		this.mainDefinition = mainDefinition;
		this.capability = capability;
		parentTypeDefitionType = parentTypeDefinitionType;
		this.content = content;
	}

	public Type ResolveClrType()
	{
		return parentTypeDefitionType.datatype switch
		{
			datatype.@bool => typeof(bool), 
			datatype.@int => typeof(int), 
			datatype.@double => typeof(double), 
			datatype.@string => typeof(string), 
			_ => throw new InvalidDefinitionException(), 
		};
	}

	public bool ResolveClrTypeSupported()
	{
		switch (parentTypeDefitionType.datatype)
		{
		case datatype.@bool:
		case datatype.@int:
		case datatype.@double:
		case datatype.@string:
			return true;
		default:
			return false;
		}
	}
}
