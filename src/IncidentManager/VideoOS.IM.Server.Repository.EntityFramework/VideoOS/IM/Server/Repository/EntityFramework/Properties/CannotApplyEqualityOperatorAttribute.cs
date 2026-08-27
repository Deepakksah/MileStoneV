using System;

namespace VideoOS.IM.Server.Repository.EntityFramework.Properties;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public sealed class CannotApplyEqualityOperatorAttribute : Attribute
{
}
