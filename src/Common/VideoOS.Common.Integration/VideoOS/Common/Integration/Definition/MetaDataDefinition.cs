using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class MetaDataDefinition
{
	private MainDefinition mainDefinition;

	private metadataentryType entry;

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return entry.name;
		}
	}

	public string Type
	{
		[DebuggerStepThrough]
		get
		{
			return entry.type.ToString();
		}
	}

	public string DataType
	{
		[DebuggerStepThrough]
		get
		{
			return entry.datatype.ToString();
		}
	}

	public MetaDataDefinition(MainDefinition mainDefinition, metadataentryType entry)
	{
		this.mainDefinition = mainDefinition;
		this.entry = entry;
	}
}
