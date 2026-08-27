using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class DataContentDefinition
{
	private TypeContent parent;

	public datacontentType Base { get; private set; }

	public datatype DataType
	{
		[DebuggerStepThrough]
		get
		{
			return parent.ParentTypeDefinition.datatype;
		}
	}

	public DataContentDefinition(TypeContent parent, datacontentType datacontentType)
	{
		this.parent = parent;
		Base = datacontentType;
	}

	public static bool HasIdenticalRanges(DataContentDefinition dataContentDefinition1, DataContentDefinition dataContentDefinition2)
	{
		if (dataContentDefinition1 == null)
		{
			throw new ArgumentNullException("dataContentDefinition1");
		}
		if (dataContentDefinition2 == null)
		{
			throw new ArgumentNullException("dataContentDefinition2");
		}
		if (string.Equals(dataContentDefinition1.Base.minvalue, dataContentDefinition2.Base.minvalue, StringComparison.Ordinal) && string.Equals(dataContentDefinition1.Base.maxvalue, dataContentDefinition2.Base.maxvalue, StringComparison.Ordinal))
		{
			return string.Equals(dataContentDefinition1.Base.resolution, dataContentDefinition2.Base.resolution, StringComparison.Ordinal);
		}
		return false;
	}
}
