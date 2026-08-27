using System;

namespace VideoOS.Common.Crypto;

public interface ISecretHandlerDisposable : ISecretHandler, IDisposable
{
}
