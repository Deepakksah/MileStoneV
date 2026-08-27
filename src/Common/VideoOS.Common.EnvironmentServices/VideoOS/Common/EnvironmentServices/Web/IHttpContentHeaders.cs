using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpContentHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
}
