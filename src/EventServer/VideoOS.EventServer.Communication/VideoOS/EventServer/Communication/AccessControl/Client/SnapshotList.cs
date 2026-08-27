using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[CollectionDataContract(Name = "SnapshotList", Namespace = "urn:milestone-systems", ItemName = "Snapshot")]
public class SnapshotList : List<Snapshot>
{
}
