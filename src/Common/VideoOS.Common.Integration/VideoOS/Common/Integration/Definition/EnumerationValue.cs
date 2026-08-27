using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class EnumerationValue
{
	public EnumerationContentDefinition EnumerationContentDefinition { get; private set; }

	public enumValueType Base { get; private set; }

	public string DisplayNameResolved => Base.displayname.Resolve();

	public Guid DisplayNameReferenceId => new Guid(Base.displayname.referenceid);

	public string Name => Base.name;

	public Guid ReferenceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(Base.referenceid);
		}
	}

	public EnumerationValue(EnumerationContentDefinition enumerationContentDefinition, enumValueType enumValue)
	{
		EnumerationContentDefinition = enumerationContentDefinition;
		Base = enumValue;
	}
}
