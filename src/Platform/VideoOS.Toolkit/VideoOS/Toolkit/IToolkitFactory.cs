using System;

namespace VideoOS.Toolkit;

public interface IToolkitFactory : IDisposable
{
	IToolkit CreateInstance(string configurationXml);

	void DeleteInstance(IToolkit toolkit);
}
