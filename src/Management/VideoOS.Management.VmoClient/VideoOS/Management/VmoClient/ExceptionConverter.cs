using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Authentication;
using System.ServiceModel;
using System.ServiceModel.Security;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public class ExceptionConverter
{
	public static Exception GetConnectionConvertedException(Exception ex, string serverName, int serverPort)
	{
		return GetConnectionConvertedException(ex, serverName, serverPort, legacy: false);
	}

	public static Exception GetConnectionConvertedException(Exception ex, string serverName, int serverPort, bool legacy)
	{
		if (!(ex is TimeoutException inner))
		{
			if (!(ex is FaultException<VMOExceptionDetail> ex2))
			{
				if (!(ex is FaultException<ExceptionDetail> ex3))
				{
					if (!(ex is FaultException ex4))
					{
						if (!(ex is EndpointNotFoundException innerException))
						{
							if (!(ex is ServerTooBusyException innerException2))
							{
								CommunicationException ex6;
								if (!(ex is MessageSecurityException ex5))
								{
									if (ex is SecurityNegotiationException innerException3)
									{
										return new UserNamePasswordException(innerException3);
									}
									ex6 = ex as CommunicationException;
									if (ex6 == null)
									{
										return null;
									}
								}
								else
								{
									if (legacy || ex5.InnerException is UnauthorizedException)
									{
										return new UserNamePasswordException(ex5);
									}
									ex6 = (CommunicationException)ex;
								}
								if (ex6.InnerException is LockedOutException)
								{
									return new UserLockoutException(ex6.InnerException);
								}
								CommunicationException ex7 = ex6;
								if (ex7.InnerException is ChangePasswordException)
								{
									return new UserChangePasswordException(ex7.InnerException);
								}
								CommunicationException ex8 = ex6;
								if (ex8.InnerException is WebException && ex8.InnerException.InnerException is AuthenticationException)
								{
									return new VmoException(ex8.InnerException.InnerException.Message, ex8.InnerException.InnerException);
								}
								CommunicationException ex9 = ex6;
								return new VmoException(ex9.Message, ex9);
							}
							return new ConnectionFailureException(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", serverName, serverPort), innerException2);
						}
						return new ConnectionFailureException(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", serverName, serverPort), innerException);
					}
					return ConvertException(ex4);
				}
				return ConvertException(ex3);
			}
			return ConvertException(ex2);
		}
		return new VmoTimeoutException(Resources.OperationTimeout, inner);
	}

	public static VmoException ConvertException(FaultException ex)
	{
		if (ex.Code != null && !string.IsNullOrEmpty(ex.Code.Name))
		{
			return ConvertException(ex.Code.Name, ex.Message, ex);
		}
		return new VmoException(ex.Message, ex);
	}

	private static string GetLocalMessage(FaultException<VMOExceptionDetail> faultEx)
	{
		string vMOErrorId = faultEx.Detail.VMOErrorId;
		if (!string.IsNullOrEmpty(vMOErrorId))
		{
			CultureInfo culture = VmoClient.Environment.ResolveUICulture();
			string errorMessage = VmoResourcesLookup.GetErrorMessage(vMOErrorId, culture);
			if (!string.IsNullOrEmpty(errorMessage))
			{
				List<string> list = new List<string>();
				for (int i = 0; i < faultEx.Detail.MsgParams.Count; i++)
				{
					MessageParameter messageParameter = faultEx.Detail.MsgParams[i];
					if (!messageParameter.Localizable)
					{
						list.Add(messageParameter.Value);
						continue;
					}
					string messageParameterString = VmoResourcesLookup.GetMessageParameterString(messageParameter.ResourceName, culture);
					if (messageParameterString != null)
					{
						list.Add(messageParameterString);
					}
					else
					{
						list.Add(messageParameter.Value);
					}
				}
				try
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					object[] args = list.ToArray();
					return string.Format(currentCulture, errorMessage, args);
				}
				catch (FormatException)
				{
					return errorMessage;
				}
			}
		}
		return faultEx.Message;
	}

	private static VmoException ConvertException(string serverSideEexceptionType, string message, FaultException faultEx)
	{
		if (faultEx is FaultException<VMOExceptionDetail> faultEx2)
		{
			message = GetLocalMessage(faultEx2);
			VmoException ex = ConvertFromErrorId(message, faultEx2);
			if (ex != null)
			{
				return ex;
			}
		}
		return ConvertFromServerSideExceptionType(serverSideEexceptionType, message, faultEx);
	}

	private static VmoException ConvertFromErrorId(string message, FaultException<VMOExceptionDetail> faultEx)
	{
		string vMOErrorId = faultEx.Detail.VMOErrorId;
		if (string.IsNullOrEmpty(vMOErrorId))
		{
			return null;
		}
		if (vMOErrorId == "VMO60311")
		{
			return new NotSupportedVmoException(message, faultEx);
		}
		return null;
	}

	private static VmoException ConvertFromServerSideExceptionType(string serverSideEexceptionType, string message, FaultException faultEx)
	{
		return serverSideEexceptionType switch
		{
			"ServerGroupCreationException" => new GroupCreationException(message, faultEx), 
			"ServerGroupNoGroupException" => new ServerGroupNoGroupException(message, faultEx), 
			"ServerGroupMaxDepthExceeded" => new ServerGroupMaxDepthExceededException(message, faultEx), 
			"ServerGroupUpdateException" => new ServerGroupUpdateException(message, faultEx), 
			"ServerGroupRenameException" => new GroupRenameException(message, faultEx), 
			"ServerGroupDeleteException" => new RemoveNonExistingGroupException(message, faultEx), 
			"ServerGroupRemovePrivateException" => new ServerGroupRemovePrivateException(message, faultEx), 
			"ServerGroupDeleteHasMembersException" => new ServerGroupDeleteHasMembersException(message, faultEx), 
			"ServerGroupDeletePrimaryException" => new ServerGroupDeletePrimaryException(message, faultEx), 
			"ServerGroupNotFoundException" => new ServerGroupNotFoundException(message, faultEx), 
			"AddMemberIdentityMissingException" => new AddMemberIdentityMissingException(message, faultEx), 
			"AddMemberIdentityAlreadyMemberException" => new AddMemberAlreadyMemberException(message, faultEx), 
			"InvalidSidException" => new InvalidSidException(message, faultEx), 
			"RoleCreationException" => new RoleCreationException(message, faultEx), 
			"RoleUpdateException" => new RoleUpdateException(message, faultEx), 
			"RoleRenameException" => new RoleRenameException(message, faultEx), 
			"RemoveNonExistingRoleException" => new RemoveNonExistingRoleException(message, faultEx), 
			"RemoveBuiltinRoleException" => new RemoveBuiltinRoleException(message, faultEx), 
			"RemoveRoleMemberNotMemberException" => new RemoveRoleMemberNotMemberException(message, faultEx), 
			"UserDefinedEventCreationException" => new UserDefinedEventCreationException(message, faultEx), 
			"UserDefinedEventRenameException" => new UserDefinedEventRenameException(message, faultEx), 
			"UserDefinedEventUpdateException" => new UserDefinedEventUpdateException(message, faultEx), 
			"UserDefinedEventNotFoundException" => new UserDefinedEventNotFoundException(message, faultEx), 
			"RemoveNonExistingUserDefinedEventException" => new RemoveNonExistingUserDefinedEventException(message, faultEx), 
			"PluginEventCreationException" => new PluginEventCreationException(message, faultEx), 
			"PluginEventRenameException" => new PluginEventRenameException(message, faultEx), 
			"PluginEventUpdateException" => new PluginEventUpdateException(message, faultEx), 
			"PluginEventNotFoundException" => new PluginEventNotFoundException(message, faultEx), 
			"RemoveNonExistingPluginEventException" => new RemoveNonExistingPluginEventException(message, faultEx), 
			"TimeProfileCreationException" => new TimeProfileCreationException(message, faultEx), 
			"TimeProfileRenameException" => new TimeProfileRenameException(message, faultEx), 
			"TimeProfileUpdateException" => new TimeProfileUpdateException(message, faultEx), 
			"RemoveNonExistingTimeProfileException" => new RemoveNonExistingTimeProfileException(message, faultEx), 
			"VideoWallCreationException" => new VideoWallCreationException(message, faultEx), 
			"VideoWallRenameException" => new VideoWallRenameException(message, faultEx), 
			"VideoWallUpdateException" => new VideoWallUpdateException(message, faultEx), 
			"RemoveNonExistingVideoWallException" => new RemoveNonExistingVideoWallException(message, faultEx), 
			"VideoWallNotFoundException" => new VideoWallNotFoundException(message, faultEx), 
			"VideoWallPresetCreationException" => new VideoWallPresetCreationException(message, faultEx), 
			"VideoWallPresetRenameException" => new VideoWallPresetRenameException(message, faultEx), 
			"VideoWallPresetUpdateException" => new VideoWallPresetUpdateException(message, faultEx), 
			"VideoWallPresetNotFoundException" => new VideoWallPresetNotFoundException(message, faultEx), 
			"RemoveNonExistingVideoWallPresetException" => new RemoveNonExistingVideoWallPresetException(message, faultEx), 
			"VideoWallPresetNoVideoWallException" => new VideoWallPresetNoVideoWallException(message, faultEx), 
			"MonitorPresetUpdateException" => new MonitorPresetUpdateException(message, faultEx), 
			"MonitorCreationException" => new MonitorCreationException(message, faultEx), 
			"MonitorRenameException" => new MonitorRenameException(message, faultEx), 
			"MonitorUpdateException" => new MonitorUpdateException(message, faultEx), 
			"RemoveNonExistingMonitorException" => new RemoveNonExistingMonitorException(message, faultEx), 
			"MonitorNotFoundException" => new MonitorNotFoundException(message, faultEx), 
			"ViewCreationException" => new ViewCreationException(message, faultEx), 
			"ViewUpdateException" => new ViewUpdateException(message, faultEx), 
			"ViewRenameException" => new ViewRenameException(message, faultEx), 
			"RemoveNonExistingViewException" => new RemoveNonExistingViewException(message, faultEx), 
			"ViewNotFoundException" => new ViewNotFoundException(message, faultEx), 
			"ViewItemCreationException" => new ViewItemCreationException(message, faultEx), 
			"ViewItemUpdateException" => new ViewItemUpdateException(message, faultEx), 
			"FeatureNotInstalledException" => new FeatureNotInstalledException(message, faultEx), 
			"SiteRemoteSiteHasMasterException" => new SiteRemoteSiteHasMasterException(message, faultEx), 
			"SiteAlreadyAttachedException" => new SiteAlreadyAttachedException(message, faultEx), 
			"SiteToSiteConnectionFailureException" => new SiteToSiteConnectionFailureException(message, faultEx), 
			"SiteToSiteUnauthorizedAccessException" => new SiteToSiteUnauthorizedAccessException(message, faultEx), 
			"SiteMasterSiteDoesNotHaveHomeSiteAsSlave" => new SiteMasterSiteDoesNotHaveHomeSiteAsChildException(message, faultEx), 
			"SiteNotFoundException" => new SiteNotFoundException(message, faultEx), 
			"SiteMasterAlreadyAttachedException" => new SiteMasterAlreadyAttachedException(message, faultEx), 
			"UnauthorizedAccessException" => new VmoUnauthorizedAccessException(message, faultEx), 
			"51000" => new VmoInsufficientPermissionsException(message, faultEx), 
			"VmoUnauthorizedAccessException" => new VmoUnauthorizedAccessException(message, faultEx), 
			"InvalidServerAddressException" => new InvalidServerAddressException(message, faultEx), 
			"ManagementServerServiceUnavailableException" => new ManagementServerServiceUnavailableException(message, faultEx), 
			"RecordingServerInformationException" => new RecordingServerInformationException(message, faultEx), 
			"RecordingServerNotFoundException" => new RecordingServerNotFoundException(message, faultEx), 
			"RecordingServerUpdateException" => new RecordingServerNotFoundException(message, faultEx), 
			"RecordingServerCreateException" => new RecordingServerCreateException(message, faultEx), 
			"UnlicensedRecordersException" => new RecordingServerUnlicensedException(message, faultEx), 
			"RecordingServerCommunicationException" => new RecordingServerCommunicationException(message, faultEx), 
			"RecordingStorageInformationException" => new RecordingStorageInformationException(message, faultEx), 
			"ArchiveStorageInformationException" => new ArchiveStorageInformationException(message, faultEx), 
			"ArchiveStorageLimitException" => new ArchiveStorageLimitException(message, faultEx), 
			"CustomPropertyNotFoundException" => new CustomPropertyNotFoundException(message, faultEx), 
			"DatabaseException" => new ConnectionException(VmoResources.VMO60000, faultEx), 
			"InvalidServerIdException" => new InvalidServerIdException(message, faultEx), 
			"InvalidServerVersionException" => new InvalidServerVersionException(message, faultEx), 
			"ConnectionFailureException" => new ConnectionFailureException(message, faultEx), 
			"HostingServerCreationException" => new HostingServerCreationException(message, faultEx), 
			"HostingServerUpdateException" => new HostingServerUpdateException(message, faultEx), 
			"HostingServerUpdateNotExistException" => new HostingServerUpdateNotExistException(message, faultEx), 
			"HostingServerDeleteNotExistException" => new HostingServerDeleteNotExistException(message, faultEx), 
			"HostingServerDeleteNotEmptyException" => new HostingServerDeleteNotEmptyException(message, faultEx), 
			"HostedHardwareCreationException" => new HostedHardwareCreationException(message, faultEx), 
			"HostedHardwareUpdateException" => new HostedHardwareUpdateException(message, faultEx), 
			"HostedHardwareUpdateNotExistException" => new HostedHardwareUpdateNotExistException(message, faultEx), 
			"HostedHardwareDeleteNotExistException" => new HostedHardwareDeleteNotExistException(message, faultEx), 
			"UnlicensedArchiveReducedFramerateFeatureException" => new UnlicensedArchiveReducedFramerateFeatureException(message, faultEx), 
			"UnlicensedTimeControlledAccessRightsFeatureException" => new UnlicensedTimeControlledAccessRightsFeatureException(message, faultEx), 
			"UnlicensedAxisOneClickFeatureException" => new UnlicensedAxisOneClickFeatureException(message, faultEx), 
			"UnlicensedFederatedSitesFeatureException" => new UnlicensedFederatedSitesFeatureException(message, faultEx), 
			"UnlicensedInterconnectedProductsFeatureException" => new UnlicensedInterconnectedProductsFeatureException(message, faultEx), 
			"UnlicensedLiveMultistreamingFeatureException" => new UnlicensedLiveMultistreamingFeatureException(message, faultEx), 
			"UnlicensedMulticastFeatureException" => new UnlicensedMulticastFeatureException(message, faultEx), 
			"UnlicensedMultistageVideoStorageFeatureException" => new UnlicensedMultistageVideoStorageFeatureException(message, faultEx), 
			"UnlicensedRecordingServerFailoverFeatureException" => new UnlicensedRecordingServerFailoverFeatureException(message, faultEx), 
			"UnlicensedRecordingStorageEncryptionFeatureException" => new UnlicensedRecordingStorageEncryptionFeatureException(message, faultEx), 
			"UnlicensedRecordingStorageSigningFeatureException" => new UnlicensedRecordingStorageSigningFeatureException(message, faultEx), 
			"UnlicensedRemoteConnectedServicesFeatureException" => new UnlicensedRemoteConnectedServicesFeatureException(message, faultEx), 
			"UnlicensedRuleBasedBookmarkingFeatureException" => new UnlicensedRuleBasedBookmarkingFeatureException(message, faultEx), 
			"UnlicensedBookmarkingFeatureException" => new UnlicensedBookmarkingFeatureException(message, faultEx), 
			"UnlicensedSmartClientProfilesFeatureException" => new UnlicensedSmartClientProfilesFeatureException(message, faultEx), 
			"UnlicensedSmartWallFeatureException" => new UnlicensedSmartWallFeatureException(message, faultEx), 
			"UnlicensedSnmpFeatureException" => new UnlicensedSnmpFeatureException(message, faultEx), 
			"UnlicensedMarkedDataFeatureException" => new UnlicensedMarkedDataFeatureException(message, faultEx), 
			"UnlicensedEvidenceLockFeatureException" => new UnlicensedEvidenceLockFeatureException(message, faultEx), 
			"UnlicensedExtendedPtzAnonymousPtzFeatureException" => new UnlicensedExtendedPtzAnonymousPtzFeatureException(message, faultEx), 
			"UnlicensedExtendedPtzLockedPresetFeatureException" => new UnlicensedExtendedPtzLockedPresetFeatureException(message, faultEx), 
			"FeatureNotLicensedException" => new FeatureNotLicensedException(message, faultEx), 
			"LicenseExpiredException" => new LicenseExpiredException(message, faultEx), 
			"LicenseFileInvalidException" => new LicenseFileInvalidException(message, faultEx), 
			"BasicUserSecureConnectionException" => new BasicUserSecureConnectionException(message, faultEx), 
			"BasicUserCreationException" => new BasicUserCreationException(message, faultEx), 
			"BasicUserUpdateException" => new BasicUserUpdateException(message, faultEx), 
			"BasicUserRenameException" => new BasicUserRenameException(message, faultEx), 
			"BasicUserDeleteException" => new BasicUserDeleteException(message, faultEx), 
			"BasicUserNotExistException" => new BasicUserNotExistException(message, faultEx), 
			"TaskCleanupNotAllowedException" => new TaskCleanupNotAllowedException(message, faultEx), 
			"BasicUserPasswordException" => new BasicUserPasswordException(message, faultEx), 
			"BasicUserPasswordSameAsOldException" => new BasicUserOldPasswordException(message, faultEx), 
			"BasicOwnerInfoCreationException" => new BasicOwnerInfoCreationException(message, faultEx), 
			"BasicOwnerInfoUpdateException" => new BasicOwnerInfoUpdateException(message, faultEx), 
			"BasicOwnerInfoDeleteException" => new BasicOwnerInfoDeleteException(message, faultEx), 
			"BasicOwnerInfoNotExistException" => new BasicOwnerInfoNotExistException(message, faultEx), 
			"RetentionOptionRoleRelationCreationException" => new RetentionOptionRoleRelationCreationException(message, faultEx), 
			"RetentionOptionRoleRelationDeleteException" => new RetentionOptionRoleRelationDeleteException(message, faultEx), 
			"RetentionOptionRoleRelationNotExistException" => new RetentionOptionRoleRelationNotExistException(message, faultEx), 
			"DeviceNotFoundException" => new DeviceNotFoundException(message, faultEx), 
			"DevicePtzPresetNotFoundException" => new DevicePtzPresetNotFoundException(message, faultEx), 
			"MIPObjectNotFoundException" => new MIPObjectNotFoundException(message, faultEx), 
			"MIPActionNotFoundException" => new MIPActionNotFoundException(message, faultEx), 
			"MIPActionDescriptionTextNotValidException" => new MIPActionDescriptionTextNotValidException(message, faultEx), 
			"ClientProfileCreationException" => new ClientProfileCreationException(message, faultEx), 
			"ClientProfileUpdateException" => new ClientProfileUpdateException(message, faultEx), 
			"ClientProfileRenameException" => new ClientProfileRenameException(message, faultEx), 
			"ClientProfileDeleteException" => new ClientProfileDeleteException(message, faultEx), 
			"ClientProfileNotExistException" => new ClientProfileNotExistException(message, faultEx), 
			"PatrollingProfileCreationException" => new PatrollingProfileCreationException(message, faultEx), 
			"PatrollingProfileUpdateException" => new PatrollingProfileUpdateException(message, faultEx), 
			"PatrollingProfileRenameException" => new PatrollingProfileRenameException(message, faultEx), 
			"RemoveNonExistingPatrollingProfileException" => new RemoveNonExistingPatrollingProfileException(message, faultEx), 
			"PatrollingProfileNotFoundException" => new PatrollingProfileNotFoundException(message, faultEx), 
			"PatrollingProfilePtzNotSupportedException" => new PatrollingProfilePtzNotSupportedException(message, faultEx), 
			_ => new VmoException(message, faultEx), 
		};
	}
}
