using System;

namespace VideoOS.IM.Server.Service.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public sealed class CannotApplyEqualityOperatorAttribute : Attribute
{
}
