using System;
using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public interface IInnerRegistration
{
	bool Initialize(Dictionary<Type, object> results);

	bool Register(Dictionary<Type, object> results);
}
