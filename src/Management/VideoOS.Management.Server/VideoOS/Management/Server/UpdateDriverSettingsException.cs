using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public abstract class UpdateDriverSettingsException : AdministrationSubsystemException
{
	public string Hwid { get; private set; }

	public string CapabilityName { get; private set; }

	public Guid TypeReferenceId { get; private set; }

	protected UpdateDriverSettingsException(string message, string hwid, string capabilityName, Guid typeReferenceId, params MessageParameter[] parameters)
		: base(message)
	{
		if (parameters != null)
		{
			foreach (MessageParameter messageParameter in parameters)
			{
				base.MessageParameters.Add(messageParameter.Index, messageParameter);
			}
		}
		Hwid = hwid;
		CapabilityName = capabilityName;
		TypeReferenceId = typeReferenceId;
	}

	protected UpdateDriverSettingsException()
		: base(VmoResources.VMO60196)
	{
	}

	protected UpdateDriverSettingsException(string message)
		: base(message)
	{
	}

	protected UpdateDriverSettingsException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDriverSettingsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		Hwid = info.GetString("Hwid");
		CapabilityName = info.GetString("CapabilityName");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("Hwid", Hwid);
		info.AddValue("CapabilityName", CapabilityName);
	}
}
