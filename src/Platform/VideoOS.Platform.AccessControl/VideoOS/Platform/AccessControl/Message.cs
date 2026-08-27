using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.Data;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl;

public class Message
{
	public class FetchConfigurationStatusChangedData
	{
		public Guid AccessControlSystemId { get; set; }

		public double Progress { get; set; }

		public string ProgressMessage { get; set; }

		public string SerializedACConfiguration { get; set; }

		public string ErrorMessage { get; set; }
	}

	public class CredentialHoldersChangedData
	{
		public IEnumerable<AccessControlCredentialHolder> AccessControlCredentialHolders { get; set; }
	}

	public class StatesChangedData
	{
		public IEnumerable<StateChangedInfo> StateChangedInfos { get; set; }
	}

	public class StateChangedInfo
	{
		public AccessControlState NewAccessControlState { get; set; }

		public AccessControlState OldAccessControlState { get; set; }
	}

	public class AccessRequestedData
	{
		public IEnumerable<AccessRequestedInfo> AccessRequestedInfos { get; set; }
	}

	public class AccessRequestedInfo
	{
		public Guid AccessControlSystemId { get; set; }

		public Guid AccessControlRuleId { get; set; }

		public BaseEvent Event { get; set; }
	}
}
