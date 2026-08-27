using NmToolkit;

namespace VideoOS.Toolkit;

public class ExportData : Data, IExportData
{
	public unsafe ExportData(ImExportData* nativeExportData)
		: base((ImData*)nativeExportData)
	{
	}
}
