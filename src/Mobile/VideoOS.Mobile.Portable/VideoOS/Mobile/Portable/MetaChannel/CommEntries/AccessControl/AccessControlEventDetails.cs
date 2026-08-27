using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlEventDetails
{
	public Guid Id { get; set; }

	public Guid AccessControlRuleId { get; set; }

	public Guid AccessControlSystemId { get; set; }

	public string AccessControlSystemName { get; set; }

	public string Message { get; set; }

	public Guid SourceId { get; set; }

	public string SourceName { get; set; }

	public DateTime Timestamp { get; set; }

	public string Reason { get; set; }

	public List<Tuple<Guid, string>> RelatedCameras { get; set; }

	public List<AccessControlCredentialHolder> CredentialHolders { get; set; }

	public List<AccessControlCommandBase> Commands { get; set; }
}
