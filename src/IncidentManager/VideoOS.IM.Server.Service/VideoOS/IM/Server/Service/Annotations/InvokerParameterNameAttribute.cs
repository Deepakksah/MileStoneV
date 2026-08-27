using System;

namespace VideoOS.IM.Server.Service.Annotations;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class InvokerParameterNameAttribute : Attribute
{
}
