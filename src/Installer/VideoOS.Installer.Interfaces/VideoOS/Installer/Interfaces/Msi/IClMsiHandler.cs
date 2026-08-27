using System;
using System.Text;

namespace VideoOS.Installer.Interfaces.Msi;

public interface IClMsiHandler
{
	bool CanceledByUser { get; set; }

	string CustomErrorMessage { get; }

	bool EnableLog(string logfile, bool enableExtendedMsiLogging);

	uint Install(string filename, string par);

	uint RemoveWithParams(string fileName, string commandLine);

	uint Remove(string productCode, string commandLine);

	void SetUI(ClMsiHandlerDelegates.UpdateProgress updateProgress, ClMsiHandlerDelegates.UpdateActionStart updateActionStart, ClMsiHandlerDelegates.UpdateActionData updateActionData);

	int UiRecordHandler(IntPtr pContext, uint uiMessageType, IntPtr hRecord);

	bool FindProductAndUpgradeCode(string pFileName, StringBuilder pProductId, int dwIdLen, StringBuilder pUpgradeCode, int dwUpgradeLen);

	string GetProductName(string fileName);

	string GetProductVersion(string fileName);

	string GetPreviousInstalledProductVersion(string fileName);

	string GetPropertyFromMsi(string fileName, string property);

	Exception GetLastError();

	StringBuilder EnumRelatedProducts(StringBuilder UpgradeCode, uint index);

	int MessageHandler(IntPtr pContext, uint msgType, StringBuilder message);

	int MsiQueryProductState(string product);

	string GetProductInfo(string prodCode, string property);
}
