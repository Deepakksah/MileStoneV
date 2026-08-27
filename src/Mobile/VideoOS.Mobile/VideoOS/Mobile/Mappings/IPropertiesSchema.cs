using System.Collections.Generic;

namespace VideoOS.Mobile.Mappings;

public interface IPropertiesSchema
{
	IEnumerable<IdGroupMapping> IdGroupProperties { get; }

	IEnumerable<ArrayMapping> ArrayProperties { get; }

	IEnumerable<Mapping> SingleProperties { get; }
}
