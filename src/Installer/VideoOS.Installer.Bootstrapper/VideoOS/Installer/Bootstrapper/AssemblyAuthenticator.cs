using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace VideoOS.Installer.Bootstrapper;

public static class AssemblyAuthenticator
{
	private static readonly byte[] _ownPublicKey = GetOwnPublicKey();

	private static byte[] GetOwnPublicKey()
	{
		return Assembly.GetExecutingAssembly().GetName().GetPublicKey();
	}

	public static void Authenticate(string filePath)
	{
		Authenticate(filePath, _ownPublicKey, null);
	}

	public static void Authenticate(string filePath, byte[] publicKeyToken)
	{
		Authenticate(filePath, null, publicKeyToken);
	}

	private static void Authenticate(string filePath, byte[] publicKey, byte[] publicKeyToken)
	{
		try
		{
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(filePath);
			byte[] publicKey2 = assemblyName.GetPublicKey();
			byte[] publicKeyToken2 = assemblyName.GetPublicKeyToken();
			if (publicKey != null && (publicKey2 == null || !publicKey2.SequenceEqual(publicKey)))
			{
				throw new SecurityException(FormattableString.Invariant(FormattableStringFactory.Create("The Assembly {0} is not strongnamed with the correct Strongname Key{1}The actual public key is:{2}{3}{4}The expected public key is{5}{6}", filePath, Environment.NewLine, Environment.NewLine, (publicKey2 != null) ? string.Join(" ", publicKey2.Select((byte z) => FormattableString.Invariant($"{z:x2}")).ToArray()) : "null", Environment.NewLine, Environment.NewLine, string.Join(" ", publicKey.Select((byte z) => FormattableString.Invariant($"{z:x2}")).ToArray()))));
			}
			if (publicKeyToken != null && (publicKeyToken2 == null || !publicKeyToken2.SequenceEqual(publicKeyToken)))
			{
				throw new SecurityException(FormattableString.Invariant(FormattableStringFactory.Create("The Assembly {0} is not strongnamed with the correct Strongname Key{1}The actual public key token is:{2}{3}{4}The expected public key token is{5}{6}", filePath, Environment.NewLine, Environment.NewLine, (publicKeyToken2 != null) ? string.Join(" ", publicKeyToken2.Select((byte z) => FormattableString.Invariant($"{z:x2}")).ToArray()) : "null", Environment.NewLine, Environment.NewLine, string.Join(" ", publicKeyToken.Select((byte z) => FormattableString.Invariant($"{z:x2}")).ToArray()))));
			}
			try
			{
				StrongNameInterop.StrongName.StrongNameSignatureVerificationEx(filePath, (sbyte)1);
			}
			catch (SecurityException)
			{
				throw new SecurityException(FormattableString.Invariant($"The Strongnaming of the Assembly {filePath} Could not be verified! It seems like this assembly is corrupt"));
			}
		}
		catch (FileNotFoundException innerException)
		{
			throw new FileNotFoundException("Specified Path = " + filePath, innerException);
		}
		catch (DirectoryNotFoundException innerException2)
		{
			throw new DirectoryNotFoundException("Specified Path = " + filePath, innerException2);
		}
	}
}
