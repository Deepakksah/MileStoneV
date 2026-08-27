using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IRecordersRequiringLicenseUpdateDictionary : IDictionary<Guid, byte[]>, ICollection<KeyValuePair<Guid, byte[]>>, IEnumerable<KeyValuePair<Guid, byte[]>>, IEnumerable
{
}
