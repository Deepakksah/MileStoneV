using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Security;

public class LegacySecurityAttributeMapper : ILegacySecurityAttributeMapper
{
	private Dictionary<string, LegacySecurityActionInformation> entries = new Dictionary<string, LegacySecurityActionInformation>();

	private Dictionary<string, LegacySecurityActionInformation> entries2 = new Dictionary<string, LegacySecurityActionInformation>();

	public static LegacySecurityAttributeMapper Instance { get; } = new LegacySecurityAttributeMapper();

	public LegacySecurityAttributeMapper()
	{
		AddEntry("DEVICEVISIBLE", "Device", SecurityNamespaceIdentifier.Camera, "GENERIC_READ");
		AddEntry("DEVICELIVEVIEW", "Device", SecurityNamespaceIdentifier.Camera, "VIEW_LIVE");
		AddEntry("DEVICELIVEVIEWRESTRICTED", "Device", SecurityNamespaceIdentifier.Camera, "VIEW_RESTRICTED_LIVE");
		AddEntry("DEVICERECORDEDBROWSE", "Device", SecurityNamespaceIdentifier.Camera, "PLAYBACK");
		AddEntry("DEVICERECORDEDBROWSERESTRICTED", "Device", SecurityNamespaceIdentifier.Camera, "PLAYBACK_RESTRICTED_RECORDINGS");
		AddEntry("DEVICEREADRESTRICTIONS", "Device", SecurityNamespaceIdentifier.Camera, "READ_RESTRICTIONS");
		AddEntry("DEVICERESTRICTIONCREATE", "Device", SecurityNamespaceIdentifier.Camera, "CREATE_RESTRICTIONS");
		AddEntry("DEVICERESTRICTIONREMOVE", "Device", SecurityNamespaceIdentifier.Camera, "DELETE_RESTRICTIONS");
		AddEntry("DEVICERECORDEDEXPORT", "Device", SecurityNamespaceIdentifier.Camera, "EXPORT");
		AddEntry("DEVICERECORDEDSMARTSEARCH", "Device", SecurityNamespaceIdentifier.Camera, "SMARTSEARCH");
		AddEntry("DEVICERECORDEDGETSEQUENCES", "Device", SecurityNamespaceIdentifier.Camera, "READ_SEQUENCES");
		AddEntry("DEVICEBOOKMARKVIEW", "Device", SecurityNamespaceIdentifier.Camera, "READ_BOOKMARKS");
		AddEntry("DEVICEBOOKMARKADD", "Device", SecurityNamespaceIdentifier.Camera, "CREATE_BOOKMARKS");
		AddEntry("DEVICEBOOKMARKEDIT", "Device", SecurityNamespaceIdentifier.Camera, "EDIT_BOOKMARKS");
		AddEntry("DEVICEBOOKMARKDELETE", "Device", SecurityNamespaceIdentifier.Camera, "DELETE_BOOKMARKS");
		AddEntry("DEVICEMARKEDDATAVIEW", "Device", SecurityNamespaceIdentifier.Camera, "READ_EVIDENCE_LOCK");
		AddEntry("DEVICEMARKEDDATACREATE", "Device", SecurityNamespaceIdentifier.Camera, "CREATE_EVIDENCE_LOCK");
		AddEntry("DEVICEMARKEDDATAREMOVE", "Device", SecurityNamespaceIdentifier.Camera, "DELETE_EVIDENCE_LOCK");
		AddEntry("DEVICEPTZAUX", "Device", SecurityNamespaceIdentifier.Camera, "AUX_COMMANDS");
		AddEntry("DEVICERETRIEVEEDGERECORDINGS", "Device", SecurityNamespaceIdentifier.Camera, "READ_REMOTE_RECORDINGS");
		AddEntry("DEVICEMANUALRECORDINGSTART", "Device", SecurityNamespaceIdentifier.Camera, "START_MANUAL_RECORDING");
		AddEntry("DEVICEMANUALRECORDINGSTOP", "Device", SecurityNamespaceIdentifier.Camera, "STOP_MANUAL_RECORDING");
		AddEntry("DEVICEINPUTVISIBLE", "Device", SecurityNamespaceIdentifier.Input, "GENERIC_READ");
		AddEntry("DEVICEOUTPUTVISIBLE", "Device", SecurityNamespaceIdentifier.Output, "GENERIC_READ");
		AddEntry("DEVICEACTIVEOUTPUT", "Device", SecurityNamespaceIdentifier.Output, "ACTIVATE");
		AddEntry("DEVICEMICVISIBLE", "Device", SecurityNamespaceIdentifier.Microphone, "GENERIC_READ");
		AddEntry("DEVICEMICLIVEVIEW", "Device", SecurityNamespaceIdentifier.Microphone, "LISTEN");
		AddEntry("DEVICEMICLIVEVIEWRESTRICTED", "Device", SecurityNamespaceIdentifier.Microphone, "LISTEN_RESTRICTED_LIVE");
		AddEntry("DEVICEMICRECORDEDBROWSE", "Device", SecurityNamespaceIdentifier.Microphone, "PLAYBACK");
		AddEntry("DEVICEMICRECORDEDBROWSERESTRICTED", "Device", SecurityNamespaceIdentifier.Microphone, "PLAYBACK_RESTRICTED_RECORDINGS");
		AddEntry("DEVICEMICREADRESTRICTIONS", "Device", SecurityNamespaceIdentifier.Microphone, "READ_RESTRICTIONS");
		AddEntry("DEVICEMICRESTRICTIONCREATE", "Device", SecurityNamespaceIdentifier.Microphone, "CREATE_RESTRICTIONS");
		AddEntry("DEVICEMICRESTRICTIONREMOVE", "Device", SecurityNamespaceIdentifier.Microphone, "DELETE_RESTRICTIONS");
		AddEntry("DEVICEMICRECORDEDEXPORT", "Device", SecurityNamespaceIdentifier.Microphone, "EXPORT");
		AddEntry("DEVICEMICRECORDEDGETSEQUENCES", "Device", SecurityNamespaceIdentifier.Microphone, "READ_SEQUENCES");
		AddEntry("DEVICEMICBOOKMARKVIEW", "Device", SecurityNamespaceIdentifier.Microphone, "READ_BOOKMARKS");
		AddEntry("DEVICEMICBOOKMARKADD", "Device", SecurityNamespaceIdentifier.Microphone, "CREATE_BOOKMARKS");
		AddEntry("DEVICEMICBOOKMARKEDIT", "Device", SecurityNamespaceIdentifier.Microphone, "EDIT_BOOKMARKS");
		AddEntry("DEVICEMICBOOKMARKDELETE", "Device", SecurityNamespaceIdentifier.Microphone, "DELETE_BOOKMARKS");
		AddEntry("DEVICEMICMARKEDDATAVIEW", "Device", SecurityNamespaceIdentifier.Microphone, "READ_EVIDENCE_LOCK");
		AddEntry("DEVICEMICMARKEDDATACREATE", "Device", SecurityNamespaceIdentifier.Microphone, "CREATE_EVIDENCE_LOCK");
		AddEntry("DEVICEMICMARKEDDATAREMOVE", "Device", SecurityNamespaceIdentifier.Microphone, "DELETE_EVIDENCE_LOCK");
		AddEntry("DEVICEMICRETRIEVEEDGERECORDINGS", "Device", SecurityNamespaceIdentifier.Microphone, "READ_REMOTE_RECORDINGS");
		AddEntry("DEVICEMICMANUALRECORDINGSTART", "Device", SecurityNamespaceIdentifier.Microphone, "START_MANUAL_RECORDING");
		AddEntry("DEVICEMICMANUALRECORDINGSTOP", "Device", SecurityNamespaceIdentifier.Microphone, "STOP_MANUAL_RECORDING");
		AddEntry("DEVICESPEAKERVISIBLE", "Device", SecurityNamespaceIdentifier.Speaker, "GENERIC_READ");
		AddEntry("DEVICESPEAKERLIVEVIEW", "Device", SecurityNamespaceIdentifier.Speaker, "LISTEN");
		AddEntry("DEVICESPEAKERLIVEVIEWRESTRICTED", "Device", SecurityNamespaceIdentifier.Speaker, "LISTEN_RESTRICTED_LIVE");
		AddEntry("DEVICESPEAKERRECORDEDBROWSE", "Device", SecurityNamespaceIdentifier.Speaker, "PLAYBACK");
		AddEntry("DEVICESPEAKERRECORDEDBROWSERESTRICTED", "Device", SecurityNamespaceIdentifier.Speaker, "PLAYBACK_RESTRICTED_RECORDINGS");
		AddEntry("DEVICESPEAKERREADRESTRICTIONS", "Device", SecurityNamespaceIdentifier.Speaker, "READ_RESTRICTIONS");
		AddEntry("DEVICESPEAKERRESTRICTIONCREATE", "Device", SecurityNamespaceIdentifier.Speaker, "CREATE_RESTRICTIONS");
		AddEntry("DEVICESPEAKERRESTRICTIONREMOVE", "Device", SecurityNamespaceIdentifier.Speaker, "DELETE_RESTRICTIONS");
		AddEntry("DEVICESPEAKERRECORDEDEXPORT", "Device", SecurityNamespaceIdentifier.Speaker, "EXPORT");
		AddEntry("DEVICESPEAKERRECORDEDGETSEQUENCES", "Device", SecurityNamespaceIdentifier.Speaker, "READ_SEQUENCES");
		AddEntry("DEVICESPEAKERBOOKMARKVIEW", "Device", SecurityNamespaceIdentifier.Speaker, "READ_BOOKMARKS");
		AddEntry("DEVICESPEAKERBOOKMARKADD", "Device", SecurityNamespaceIdentifier.Speaker, "CREATE_BOOKMARKS");
		AddEntry("DEVICESPEAKERBOOKMARKEDIT", "Device", SecurityNamespaceIdentifier.Speaker, "EDIT_BOOKMARKS");
		AddEntry("DEVICESPEAKERBOOKMARKDELETE", "Device", SecurityNamespaceIdentifier.Speaker, "DELETE_BOOKMARKS");
		AddEntry("DEVICESPEAKERMARKEDDATAVIEW", "Device", SecurityNamespaceIdentifier.Speaker, "READ_EVIDENCE_LOCK");
		AddEntry("DEVICESPEAKERMARKEDDATACREATE", "Device", SecurityNamespaceIdentifier.Speaker, "CREATE_EVIDENCE_LOCK");
		AddEntry("DEVICESPEAKERMARKEDDATAREMOVE", "Device", SecurityNamespaceIdentifier.Speaker, "DELETE_EVIDENCE_LOCK");
		AddEntry("DEVICESPEAKERRETRIEVEEDGERECORDINGS", "Device", SecurityNamespaceIdentifier.Speaker, "READ_REMOTE_RECORDINGS");
		AddEntry("DEVICESPEAKERMANUALRECORDINGSTART", "Device", SecurityNamespaceIdentifier.Speaker, "START_MANUAL_RECORDING");
		AddEntry("DEVICESPEAKERMANUALRECORDINGSTOP", "Device", SecurityNamespaceIdentifier.Speaker, "STOP_MANUAL_RECORDING");
		AddEntry("DEVICEMETADATAVISIBLE", "Device", SecurityNamespaceIdentifier.Metadata, "GENERIC_READ");
		AddEntry("DEVICEMETADATALIVEVIEW", "Device", SecurityNamespaceIdentifier.Metadata, "LIVE");
		AddEntry("DEVICEMETADATALIVEVIEWRESTRICTED", "Device", SecurityNamespaceIdentifier.Metadata, "VIEW_RESTRICTED_LIVE");
		AddEntry("DEVICEMETADATARECORDEDBROWSE", "Device", SecurityNamespaceIdentifier.Metadata, "PLAYBACK");
		AddEntry("DEVICEMETADATARECORDEDBROWSERESTRICTED", "Device", SecurityNamespaceIdentifier.Metadata, "PLAYBACK_RESTRICTED_RECORDINGS");
		AddEntry("DEVICEMETADATAREADRESTRICTIONS", "Device", SecurityNamespaceIdentifier.Metadata, "READ_RESTRICTIONS");
		AddEntry("DEVICEMETADATARESTRICTIONCREATE", "Device", SecurityNamespaceIdentifier.Metadata, "CREATE_RESTRICTIONS");
		AddEntry("DEVICEMETADATARESTRICTIONREMOVE", "Device", SecurityNamespaceIdentifier.Metadata, "DELETE_RESTRICTIONS");
		AddEntry("DEVICEMETADATARECORDEDEXPORT", "Device", SecurityNamespaceIdentifier.Metadata, "EXPORT");
		AddEntry("DEVICEMETADATARECORDEDGETSEQUENCES", "Device", SecurityNamespaceIdentifier.Metadata, "READ_SEQUENCES");
		AddEntry("DEVICEMETADATARETRIEVEEDGERECORDINGS", "Device", SecurityNamespaceIdentifier.Metadata, "READ_REMOTE_RECORDINGS");
		AddEntry("DEVICEMETADATAMANUALRECORDINGSTART", "Device", SecurityNamespaceIdentifier.Metadata, "START_MANUAL_RECORDING");
		AddEntry("DEVICEMETADATAMANUALRECORDINGSTOP", "Device", SecurityNamespaceIdentifier.Metadata, "STOP_MANUAL_RECORDING");
		AddEntry("DEVICEMETADATAMARKEDDATAVIEW", "Device", SecurityNamespaceIdentifier.Metadata, "READ_EVIDENCE_LOCK");
		AddEntry("DEVICEMETADATAMARKEDDATACREATE", "Device", SecurityNamespaceIdentifier.Metadata, "CREATE_EVIDENCE_LOCK");
		AddEntry("DEVICEMETADATAMARKEDDATAREMOVE", "Device", SecurityNamespaceIdentifier.Metadata, "DELETE_EVIDENCE_LOCK");
		AddEntry("DEVICESPEAKCONTROL", "Device", SecurityNamespaceIdentifier.Speaker, "SPEAK");
		AddEntry("DEVICEPTZCONTROL", "Device", SecurityNamespaceIdentifier.Camera, "PTZ_CONTROL");
		AddEntry("DEVICEPTZPRESETS", "Device", SecurityNamespaceIdentifier.Camera, "ACTIVATE_PTZ_PRESET");
		AddEntry("DEVICEPTZMANAGE", "Device", SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET");
		AddEntry("DEVICEPTZMANAGELOCK", "Device", SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET");
		AddEntry("DEVICEPTZRESERVESESSION", "Device", SecurityNamespaceIdentifier.Camera, "RESERVE_PTZ_SESSION");
		AddEntry("DEVICEPTZRELEASESESSION", "Device", SecurityNamespaceIdentifier.Camera, "RELEASE_PTZ_SESSION");
		AddEntry("MATRIXVISIBLE", "Matrix", SecurityNamespaceIdentifier.Matrix, "GENERIC_READ");
		AddEntry("VIEWGROUPVISIBLE", "ViewGroup", SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ");
		AddEntry("APPLICATIONACCESSSTATUSAPI", "Application", SecurityNamespaceIdentifier.ManagementServer, "STATUS_API");
		AddEntry("HARDWAREEDIT", "Group", SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE");
		AddEntry("HARDWARECOMMAND", "Group", SecurityNamespaceIdentifier.Hardware, "HARDWARECOMMAND");
	}

	public LegacySecurityActionInformation GetSecurityActionInformation(string legacySecurityAttributeId)
	{
		if (!entries.TryGetValue(legacySecurityAttributeId, out var value))
		{
			return null;
		}
		return value;
	}

	public LegacySecurityActionInformation GetSecurityActionInformation(Guid namespaceId, string securityAction)
	{
		if (!entries2.TryGetValue(LegacySecurityActionInformation.GenerateSecurityActionKey(namespaceId, securityAction), out var value))
		{
			return null;
		}
		return value;
	}

	private void AddEntry(string legacySecurityAttribute, string legacySecurityArea, Guid securityNamespaceId, string securityAction)
	{
		LegacySecurityActionInformation legacySecurityActionInformation = new LegacySecurityActionInformation(legacySecurityAttribute, legacySecurityArea, securityNamespaceId, securityAction);
		entries.Add(legacySecurityActionInformation.LegacySecurityAttribute, legacySecurityActionInformation);
		try
		{
			entries2.Add(legacySecurityActionInformation.SecurityActionKey, legacySecurityActionInformation);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
