using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpResponseHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
}
