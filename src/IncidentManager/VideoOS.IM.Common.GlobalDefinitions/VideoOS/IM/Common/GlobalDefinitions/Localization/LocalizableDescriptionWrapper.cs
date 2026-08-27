using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.IM.Common.GlobalDefinitions.Localization;

public sealed class LocalizableDescriptionWrapper
{
	public string Key { get; }

	public IEnumerable<LocalizableDescriptionParameter> Parameters { get; }

	public IEnumerable<LocalizableDescriptionWrapper> ChildEntries { get; }

	public LocalizableDescriptionWrapper(string key, IEnumerable<LocalizableDescriptionParameter> parameters = null, IEnumerable<LocalizableDescriptionWrapper> childEntries = null)
	{
		Key = key ?? throw new ArgumentNullException("key");
		Parameters = parameters ?? Enumerable.Empty<LocalizableDescriptionParameter>();
		ChildEntries = childEntries ?? Enumerable.Empty<LocalizableDescriptionWrapper>();
	}

	public override string ToString()
	{
		return $"{Key} - {(Parameters as ICollection<LocalizableDescriptionParameter>)?.Count ?? 0} - {(ChildEntries as ICollection<LocalizableDescriptionWrapper>)?.Count ?? 0}";
	}
}
