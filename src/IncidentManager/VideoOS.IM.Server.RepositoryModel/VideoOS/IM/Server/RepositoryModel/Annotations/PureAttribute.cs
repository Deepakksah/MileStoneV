using System;

namespace VideoOS.IM.Server.RepositoryModel.Annotations;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class PureAttribute : Attribute
{
}
