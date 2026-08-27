using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriverEventParser : IDisposable
{
	bool IsOpen { get; }

	bool Open();

	void Close();

	bool ParseData(byte[] data, out string eventResultXml);

	bool ParseEventResult(string eventResultXml, out string eventXmlFragment, out string continueAction, out string savePath);

	bool ParseEventXmlFragmentInformation(string eventXmlFragment, out string sourceType, out string sourceId, out string eventId);
}
