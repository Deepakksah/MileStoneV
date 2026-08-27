using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Installer.Core.Arguments.Interfaces;

public interface IArgumentCollection : IEnumerable<IArgument>, IEnumerable
{
	IArgument this[string argumentName] { get; }

	LicenseType LicenseType { get; }
}
