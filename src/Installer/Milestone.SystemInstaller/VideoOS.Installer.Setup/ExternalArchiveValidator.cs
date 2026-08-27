using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using VideoOS.Installer.Setup.Properties;

namespace VideoOS.Installer.Setup;

public class ExternalArchiveValidator : IDisposable
{
	private readonly string _externalArchivePath;

	private readonly string _externalArchiveHash;

	private Stream _validatedStream;

	public Stream ValidatedStream => _validatedStream ?? throw new InvalidOperationException("Archive not yet validated.");

	public ExternalArchiveValidator(string externalArchivePath, string externalArchiveHash)
	{
		_externalArchivePath = externalArchivePath;
		_externalArchiveHash = externalArchiveHash;
	}

	public void Dispose()
	{
		_validatedStream?.Dispose();
		_validatedStream = null;
	}

	public string Validate(IProgress<int> progress)
	{
		if (_validatedStream != null)
		{
			throw new InvalidOperationException("Archive already validated.");
		}
		Stream stream = null;
		try
		{
			stream = File.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _externalArchivePath), FileMode.Open, FileAccess.Read, FileShare.None);
			using (SHA512 sHA = SHA512.Create())
			{
				using ObservableStream observableStream = new ObservableStream(stream, progress);
				observableStream.Begin();
				byte[] inArray = sHA.ComputeHash(observableStream);
				observableStream.End();
				if (_externalArchiveHash != Convert.ToBase64String(inArray))
				{
					return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveHashMismatch, _externalArchivePath);
				}
			}
			stream.Position = 0L;
			_validatedStream = stream;
			stream = null;
		}
		catch (ArgumentException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveNotFound, _externalArchivePath);
		}
		catch (PathTooLongException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveNotFound, _externalArchivePath);
		}
		catch (DirectoryNotFoundException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveNotFound, _externalArchivePath);
		}
		catch (FileNotFoundException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveNotFound, _externalArchivePath);
		}
		catch (NotSupportedException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveNotFound, _externalArchivePath);
		}
		catch (UnauthorizedAccessException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveReadError, _externalArchivePath);
		}
		catch (IOException)
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.ExternalArchiveReadError, _externalArchivePath);
		}
		finally
		{
			stream?.Dispose();
		}
		return null;
	}
}
