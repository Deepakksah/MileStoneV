using System;

namespace VideoOS.Management.Server.ConfigurationApi;

public interface IBasicOwnerInformationDataRepository
{
	BasicOwnerInfo[] QueryBasicUserInfo(IGeminiRequestContext requestContext);

	string GetServerId(IGeminiRequestContext requestContext);

	void AddBasicUserInfo(IGeminiRequestContext requestContext, string id, string name, string property, string order);

	void RemoveBasicUserInfo(IGeminiRequestContext requestContext, string id);

	void UpdateBasicOwnerInfo(IGeminiRequestContext requestContext, string id, string name, string value, string order, Guid sessionId);
}
