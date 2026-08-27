using System;

namespace VideoOS.IM.Server.Logger;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class InvokerParameterNameAttribute : Attribute
{
}
