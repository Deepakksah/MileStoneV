using System;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration;

public interface IXmlTransformation
{
	void Transform(XDocument doc, Func<string, byte[], string> method);
}
