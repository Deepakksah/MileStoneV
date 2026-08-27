using System;
using System.Collections.Generic;

namespace VideoOS.Toolkit;

public interface ISourceToolkit : IToolkit
{
	void Connect(ISet<string> requestedPropertyNames, out ISet<string> availablePropertyNames, TimeSpan timeout);

	void Connect(ISet<string> requestedPropertyNames, out ISet<string> availablePropertyNames);

	void Connect(TimeSpan timeout);

	void Connect();

	void Disconnect();

	void UpdateConfiguration(IList<KeyValuePair<string, string>> configurationParameterList);

	void UpdateConfiguration(string configurationParameterName, string configurationParameterValue);
}
