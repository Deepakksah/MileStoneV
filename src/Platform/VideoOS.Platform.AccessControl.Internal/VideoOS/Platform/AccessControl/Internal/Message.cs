using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Internal;

public class Message
{
	public class AccessControl
	{
		public const string StatesChangedIndication = "AccessControl.StatesChangedIndication";

		public const string StatesInvalidatedIndication = "AccessControl.StatesInvalidatedIndication";

		public const string CredentialHoldersChangedIndication = "AccessControl.CredentialHoldersChangedIndication";

		public const string CredentialHoldersInvalidatedIndication = "AccessControl.CredentialHoldersInvalidatedIndication";

		public const string FetchConfigurationStatusChangedIndication = "AccessControl.FetchConfigurationStatusChangedIndication";

		public const string FetchPersonalizedConfigurationStatusChangedIndication = "AccessControl.FetchPersonalizedConfigurationStatusChangedIndication";

		public const string AccessRequestedIndication = "AccessControl.AccessRequestedIndication";

		public const string PersonalizedConfigurationChangedIndication = "AccessControl.PersonalizedConfigurationChangedIndication";

		public const string UserCredentialsInvalidIndication = "AccessControl.UserCredentialsInvalidIndication";
	}

	public class CredentialHoldersInvalidatedData
	{
		public IEnumerable<CredentialHoldersInvalidatedInfo> CredentialHoldersInvalidatedInfos { get; set; }
	}

	public class CredentialHoldersInvalidatedInfo
	{
		public Guid AccessControlSystemId { get; set; }

		public IEnumerable<string> AccessControlCredentialHolderIds { get; set; }
	}

	public class StatesInvalidatedData
	{
		public IEnumerable<StatesInvalidatedInfo> StatesInvalidatedInfos { get; set; }
	}

	public class StatesInvalidatedInfo
	{
		public Guid AccessControlSystemId { get; set; }

		public IEnumerable<Guid> AccessControlOperationableInstanceIds { get; set; }
	}

	public class FetchPersonalizedConfigurationStatusChangedData
	{
		public Guid AccessControlSystemId { get; set; }

		public double Progress { get; set; }

		public string ProgressMessage { get; set; }

		public string ErrorMessage { get; set; }

		public string Username { get; set; }

		public bool Finished { get; set; }
	}

	public class PersonalizedConfigurationChangedData
	{
		public Guid AccessControlSystemId { get; set; }

		public string Username { get; set; }
	}

	public class UserCredentialsInvalidData
	{
		public Guid AccessControlSystemId { get; set; }

		public string Username { get; set; }

		public string ErrorMessage { get; set; }

		public int ErrorCode { get; set; }
	}

	public class AccessControlSystemConfigurationChangedData
	{
		public bool Enabled { get; set; }

		public bool Disabled { get; set; }

		public bool DoNotNotify { get; set; }

		public bool NameChanged { get; set; }

		public bool DescriptionChanged { get; set; }

		public bool PersonalizedLoginEnabledChanged { get; set; }

		public bool PropertiesChanged { get; set; }

		public bool SystemConfigurationChanged { get; set; }

		public bool CategoryMappingChanged { get; set; }

		public bool CameraMappingChanged { get; set; }

		public bool EnabledDoorsChanged { get; set; }

		public bool RuleConfigurationChanged { get; set; }

		public bool UserDefinedCategoriesChanged { get; set; }
	}
}
