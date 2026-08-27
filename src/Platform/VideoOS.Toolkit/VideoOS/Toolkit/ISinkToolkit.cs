using System;
using System.Collections.Generic;

namespace VideoOS.Toolkit;

public interface ISinkToolkit : IToolkit
{
	void Connect(TimeSpan timeout);

	void Connect();

	void Disconnect();

	void PutData(IData data);

	void UpdateConfiguration(IList<KeyValuePair<string, string>> configurationParameterList);

	void UpdateConfiguration(string configurationParameterName, string configurationParameterValue);
}
