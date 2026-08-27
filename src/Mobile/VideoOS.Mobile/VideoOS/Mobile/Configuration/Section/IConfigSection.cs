using System.Collections.Specialized;

namespace VideoOS.Mobile.Configuration.Section;

public interface IConfigSection
{
	string SectionName { get; }

	bool InitFromCollection(NameValueCollection section);

	bool ConvToCollection(NameValueCollection serverSection);
}
