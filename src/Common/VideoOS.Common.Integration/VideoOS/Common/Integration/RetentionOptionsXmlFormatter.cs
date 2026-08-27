using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class RetentionOptionsXmlFormatter
{
	private List<RetentionTimeOption> _retentionOptions;

	public List<RetentionTimeOption> RetentionOptions
	{
		get
		{
			return _retentionOptions;
		}
		set
		{
			_retentionOptions = value;
		}
	}
}
