using System;

namespace VideoOS.IM.Server.RepositoryModel.Annotations;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class InvokerParameterNameAttribute : Attribute
{
}
