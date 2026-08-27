using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public enum SmartSearchStatusType
{
	UnspecifiedError,
	SearchInProgress,
	SearchResultReady,
	SearchEndTimeReached,
	SearchCancelled
}
