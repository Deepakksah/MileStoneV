using System;

namespace VideoOS.IM.Server.RepositoryModel.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcPartialViewAttribute : PathReferenceAttribute
{
}
