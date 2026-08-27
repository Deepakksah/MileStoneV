using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public interface IExportToolkit : IToolkit
{
	public enum ExportResult
	{
		EXPORT_RESULT_DONE,
		EXPORT_RESULT_EMPTY,
		EXPORT_RESULT_CANCELLED,
		EXPORT_RESULT_DONE_WITH_SIGN_ERROR
	}

	ExportResult Export(out IData data, out string sourceToolkitConfigurationXml);

	ExportResult Export(out IData data);

	void CancelExport();

	void CancelExport([MarshalAs(UnmanagedType.U1)] bool keepExportedData);

	ExportResult EstimateSizeOfExport(out ulong estimatedSize);

	void CancelEstimation();

	int GetPercentageCompleted();

	int GetPercentageCompleted(out string itemCurrentlyProcessed);

	void UpdateConfiguration(IList<KeyValuePair<string, string>> configurationParameterList);

	void UpdateConfiguration(string configurationParameterName, string configurationParameterValue);
}
