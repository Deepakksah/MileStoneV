using System;

namespace VideoOS.EventServer.Communication.Extensions;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ValidatedNotNullAttribute : Attribute
{
}
