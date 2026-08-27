using System;

namespace VideoOS.Rules.Interfaces;

public interface IRule
{
	ISubRule StartRule { get; }

	ISubRule StopRule { get; }

	bool Enabled { get; }

	Guid ID { get; }

	string Name { get; }
}
