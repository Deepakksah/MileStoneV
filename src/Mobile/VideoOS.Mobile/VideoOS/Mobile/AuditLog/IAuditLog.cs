using System;
using VideoOS.Platform;

namespace VideoOS.Mobile.AuditLog;

public interface IAuditLog
{
	void SettingChanged(string serverName, string name, string oldValue, string newValue);

	void RegisteredDeviceNotificationsEnabledChanged(string serverName, string deviceName, string enabledState);

	void RegisteredDeviceDeleted(string serverName, string deviceName);

	void ExportLocationChanged(string serverName, string oldValue, string newValue);

	void ConfigExported(string serverName, string fileName);

	void AddedItem(string serverName, string itemName, string newItem);

	void RemovedItem(string serverName, string itemName, string oldItem);

	void UpdatedItem(string serverName, string itemName, string oldItem, string newItem);

	void ExportCreated(string serverName, InvestigationAuditLogData investigation, ExportAuditLogData export, Item camera);

	void ExportDownloaded(string serverName, InvestigationAuditLogData investigation, ExportAuditLogData export, Item camera);

	void ExportDeleted(string serverName, InvestigationAuditLogData investigation, ExportAuditLogData export, Item camera);

	void InvestigationCreated(string serverName, InvestigationAuditLogData details, Item camera);

	void InvestigationUpdated(string serverName, InvestigationAuditLogData oldDetails, InvestigationAuditLogData newDetails, Item camera);

	void InvestigationDeleted(string serverName, InvestigationAuditLogData details, Item camera);

	void VideoPushStarted(string serverName, Item camera);

	void VideoPushStopped(string serverName, Item camera);

	void TwoStepVerificationUserNotAllowed(string serverName);

	void TwoStepVerificationCodeSent(string serverName);

	void TwoStepVerificationCorrectCodeEntered(string serverName);

	void TwoStepVerificationIncorrectCodeEntered(string serverName);

	void VideoPlayback(string serverName, DateTime from, DateTime to, Item camera);

	void ClientInitiatedLogout(string serverName);

	void ServerInitiatedLogout(string serverName);
}
