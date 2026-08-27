using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DebuggerDisplay("{DisplayName}")]
[DataContract]
public class SecurityNamespaceInfo
{
	private List<ActionDefinition> _actions;

	[DataMember]
	public string NamespaceId { get; set; }

	public Guid NamespaceGuid
	{
		get
		{
			if (Guid.TryParse(NamespaceId, out var result))
			{
				return result;
			}
			return Guid.Empty;
		}
	}

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public char Separator { get; set; }

	[DataMember]
	public int Structure { get; set; }

	public bool UserDefined { get; set; }

	[DataMember]
	public List<ActionDefinition> Actions
	{
		get
		{
			return _actions;
		}
		public set
		{
			_actions = value;
		}
	}

	public SecurityNamespaceInfo()
	{
		_actions = new List<ActionDefinition>();
	}
}
