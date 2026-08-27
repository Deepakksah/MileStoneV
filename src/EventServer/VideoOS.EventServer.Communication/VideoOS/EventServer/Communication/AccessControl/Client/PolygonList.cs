using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[CollectionDataContract(Name = "PolygonList", Namespace = "urn:milestone-systems", ItemName = "Polygon")]
public class PolygonList : List<TPolygon>
{
}
