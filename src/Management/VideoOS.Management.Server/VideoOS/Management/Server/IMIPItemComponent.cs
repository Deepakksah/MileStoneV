using System;
using VideoOS.Platform;

namespace VideoOS.Management.Server;

public interface IMIPItemComponent : IDisposable
{
	Item QueryItem(Guid id);

	Item[] QueryItemsByKind(Guid kind);

	Item[] QueryItemsByKind(Guid kind, Guid parentId);

	Item[] QueryItemsByParent(Guid parentId);

	void DeleteItem(Guid id);

	void SaveItem(Item item, string modifiedUser);
}
