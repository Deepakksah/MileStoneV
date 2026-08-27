using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform;
using VideoOS.Platform.AccessControl.Internal.Data;
using VideoOS.Platform.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SearchElement", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
[KnownType(typeof(Version))]
[KnownType(typeof(GetAccessControlPluginsParams))]
[KnownType(typeof(GetAccessControlPluginsResp))]
[KnownType(typeof(AccessControlPlugin[]))]
[KnownType(typeof(AccessControlPlugin))]
[KnownType(typeof(AccessControlPropertyDefinition[]))]
[KnownType(typeof(AccessControlPropertyDefinition))]
[KnownType(typeof(Property[]))]
[KnownType(typeof(Property))]
[KnownType(typeof(GetAccessControlPluginIconsParams))]
[KnownType(typeof(GetAccessControlPluginIconsResp))]
[KnownType(typeof(AccessControlIcon[]))]
[KnownType(typeof(AccessControlIcon))]
[KnownType(typeof(GetAccessControlPluginCategoriesParams))]
[KnownType(typeof(GetAccessControlPluginCategoriesResp))]
[KnownType(typeof(AccessControlPluginCategory[]))]
[KnownType(typeof(AccessControlPluginCategory))]
[KnownType(typeof(GetAccessControlSystemsParams))]
[KnownType(typeof(GetAccessControlSystemsResp))]
[KnownType(typeof(AccessControlSystem[]))]
[KnownType(typeof(AccessControlSystem))]
[KnownType(typeof(AccessControlExternalCommand[]))]
[KnownType(typeof(AccessControlExternalCommand))]
[KnownType(typeof(ValidateAccessControlSystemPropertiesParams))]
[KnownType(typeof(ValidateAccessControlSystemPropertiesResp))]
[KnownType(typeof(AccessControlPropertyValidationResult))]
[KnownType(typeof(CreateAccessControlSystemParams))]
[KnownType(typeof(CreateAccessControlSystemResp))]
[KnownType(typeof(DeleteAccessControlSystemParams))]
[KnownType(typeof(DeleteAccessControlSystemResp))]
[KnownType(typeof(GetAccessControlSystemConfigurationParams))]
[KnownType(typeof(GetAccessControlSystemConfigurationResp))]
[KnownType(typeof(SetAccessControlSystemConfigurationParams))]
[KnownType(typeof(SetAccessControlSystemConfigurationResp))]
[KnownType(typeof(StartFetchAccessControlSystemConfigurationParams))]
[KnownType(typeof(StartFetchAccessControlSystemConfigurationResp))]
[KnownType(typeof(CancelFetchAccessControlSystemConfigurationParams))]
[KnownType(typeof(CancelFetchAccessControlSystemConfigurationResp))]
[KnownType(typeof(GetCredentialHoldersParams))]
[KnownType(typeof(GetCredentialHoldersResp))]
[KnownType(typeof(AccessControlCredentialHolder[]))]
[KnownType(typeof(AccessControlCredentialHolder))]
[KnownType(typeof(SetCredentialHolderImageParams))]
[KnownType(typeof(SetCredentialHolderImageResp))]
[KnownType(typeof(SearchCredentialHoldersParams))]
[KnownType(typeof(SearchCredentialHoldersResp))]
[KnownType(typeof(AccessControlCredentialHolderSearchResult[]))]
[KnownType(typeof(AccessControlCredentialHolderSearchResult))]
[KnownType(typeof(GetStatesParams))]
[KnownType(typeof(GetStatesResp))]
[KnownType(typeof(AccessControlState[]))]
[KnownType(typeof(AccessControlState))]
[KnownType(typeof(ExecuteCommandParams))]
[KnownType(typeof(ExecuteCommandResp))]
[KnownType(typeof(AccessControlCommandResult))]
[KnownType(typeof(GetGlobalSettingsParams))]
[KnownType(typeof(GetGlobalSettingsResp))]
[KnownType(typeof(AccessControlGlobalSettings))]
[KnownType(typeof(SetGlobalSettingsParams))]
[KnownType(typeof(SetGlobalSettingsResp))]
[KnownType(typeof(GetUserDefinedCategoriesParams))]
[KnownType(typeof(GetUserDefinedCategoriesResp))]
[KnownType(typeof(AccessControlUserDefinedCategory[]))]
[KnownType(typeof(AccessControlUserDefinedCategory))]
[KnownType(typeof(SetUserDefinedCategoriesParams))]
[KnownType(typeof(SetUserDefinedCategoriesResp))]
[KnownType(typeof(ValidateUserCredentialsParams))]
[KnownType(typeof(AccessControlUserCredential[]))]
[KnownType(typeof(AccessControlUserCredential))]
[KnownType(typeof(ValidateUserCredentialsResp))]
[KnownType(typeof(AccessControlUserCredentialValidationResult[]))]
[KnownType(typeof(AccessControlUserCredentialValidationResult))]
[KnownType(typeof(AccessControlEvent))]
[KnownType(typeof(BaseEvent))]
[KnownType(typeof(EventHeader))]
[KnownType(typeof(EventSource))]
[KnownType(typeof(FQID))]
[KnownType(typeof(ServerId))]
[KnownType(typeof(FolderType))]
[KnownType(typeof(AnalyticsEvent))]
[KnownType(typeof(RuleList))]
[KnownType(typeof(Rule))]
[KnownType(typeof(PolygonList))]
[KnownType(typeof(TPolygon))]
[KnownType(typeof(TColor))]
[KnownType(typeof(PointList))]
[KnownType(typeof(TPoint))]
[KnownType(typeof(AnalyticsObjectList))]
[KnownType(typeof(AnalyticsObject))]
[KnownType(typeof(BoundingBox))]
[KnownType(typeof(ObjectMotion))]
[KnownType(typeof(ReferenceList))]
[KnownType(typeof(Reference))]
[KnownType(typeof(SnapshotList))]
[KnownType(typeof(Snapshot))]
[KnownType(typeof(Vendor))]
[KnownType(typeof(EventData))]
[KnownType(typeof(EventSequence))]
[KnownType(typeof(FQID[]))]
[KnownType(typeof(GetDowntimesParams))]
[KnownType(typeof(GetDowntimesResp))]
[KnownType(typeof(string[]))]
[KnownType(typeof(Guid[]))]
[KnownType(typeof(byte[][]))]
[KnownType(typeof(AccessControlServiceFault))]
[KnownType(typeof(GetAccessControlEventsParams))]
[KnownType(typeof(GetAccessControlEventsResp))]
[KnownType(typeof(AccessControlEventLine[]))]
[KnownType(typeof(AccessControlEventLine))]
[KnownType(typeof(GetAccessControlEventsWithDowntimesParams))]
[KnownType(typeof(GetAccessControlEventsWithDowntimesResp))]
[KnownType(typeof(GetAccessControlEventPageParams))]
[KnownType(typeof(GetAccessControlEventPageResp))]
[KnownType(typeof(GetAccessControlEventPageWithDowntimesParams))]
[KnownType(typeof(GetAccessControlEventPageWithDowntimesResp))]
[KnownType(typeof(GetAccessControlEventCountParams))]
[KnownType(typeof(GetAccessControlEventCountResp))]
[KnownType(typeof(AccessControlDowntimeData[]))]
[KnownType(typeof(AccessControlDowntimeData))]
[KnownType(typeof(SearchGroup))]
[KnownType(typeof(LogicalOperator))]
[KnownType(typeof(SearchElement[]))]
[KnownType(typeof(SearchOperator))]
[KnownType(typeof(SearchGroup[]))]
[KnownType(typeof(SearchOrders))]
[KnownType(typeof(SearchOrder[]))]
[KnownType(typeof(SearchOrder))]
[KnownType(typeof(Direction))]
public class SearchElement : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private SearchOperator SearchOperatorField;

	private string SearchTargetField;

	private object SearchValueField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public SearchOperator SearchOperator
	{
		get
		{
			return SearchOperatorField;
		}
		set
		{
			SearchOperatorField = value;
		}
	}

	[DataMember]
	public string SearchTarget
	{
		get
		{
			return SearchTargetField;
		}
		set
		{
			SearchTargetField = value;
		}
	}

	[DataMember]
	public object SearchValue
	{
		get
		{
			return SearchValueField;
		}
		set
		{
			SearchValueField = value;
		}
	}
}
