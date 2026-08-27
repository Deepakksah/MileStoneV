using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[CollectionDataContract(Name = "AnalyticsObjectList", Namespace = "urn:milestone-systems", ItemName = "Object")]
public class AnalyticsObjectList : List<AnalyticsObject>
{
}
