using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security;
using System.Text;
using VideoOS.Installer.Setup.Properties;

namespace VideoOS.Installer.Setup;

public static class Extract
{
	private static string EmbeddedeZip = "InitialData.EmbeddedZip";

	private static char[] alphanum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

	public static string ExtractEmbeddedFiles(IEnumerable<string> fileNames, string extractFolder, IProgress<int> progressObserver)
	{
		using Stream zipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Extract).Namespace + "." + EmbeddedeZip);
		return ExtractStream(zipStream, delegate(ZipArchive archive)
		{
			ExtractFiles(archive, fileNames, extractFolder);
		}, progressObserver);
	}

	public static string ExtractEmbeddedZip(string extractFolder, IProgress<int> progressObserver)
	{
		using Stream zipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Extract).Namespace + "." + EmbeddedeZip);
		return ExtractStream(zipStream, delegate(ZipArchive archive)
		{
			ExtractToDirectory(archive, extractFolder);
		}, progressObserver);
	}

	public static string ExtractExternalZip(Stream zipStream, string extractFolder, IProgress<int> progressObserver)
	{
		return ExtractStream(zipStream, delegate(ZipArchive archive)
		{
			ExtractToDirectory(archive, extractFolder);
		}, progressObserver);
	}

	public static void ExtractToDirectory(ZipArchive archive, string extractFolder)
	{
		Directory.CreateDirectory(extractFolder);
		archive.ExtractToDirectory(extractFolder);
	}

	public static void ExtractFiles(ZipArchive archive, IEnumerable<string> files, string extractFolder)
	{
		foreach (string file in files)
		{
			ZipArchiveEntry entry = archive.GetEntry(file);
			if (entry == null)
			{
				throw new ArgumentException(FormattableString.Invariant($"File not found in archive: {file}"), "files");
			}
			if (!extractFolder.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
			{
				extractFolder += Path.DirectorySeparatorChar;
			}
			string fullPath = Path.GetFullPath(Path.Combine(extractFolder, entry.FullName));
			if (!fullPath.StartsWith(extractFolder, StringComparison.Ordinal))
			{
				throw new SecurityException(FormattableString.Invariant($"Traversel outside of the expected extraction path is forbidden. Entry name: {entry.FullName}"));
			}
			entry.ExtractToFile(fullPath, overwrite: true);
		}
	}

	private static string ExtractStream(Stream zipStream, Action<ZipArchive> extractCommand, IProgress<int> progress)
	{
		ObservableStream observableStream = new ObservableStream(zipStream, progress);
		using ZipArchive obj = new ZipArchive(observableStream, ZipArchiveMode.Read);
		try
		{
			observableStream.Begin();
			extractCommand(obj);
			observableStream.End();
		}
		catch (Exception ex)
		{
			return FormattableString.Invariant($"{Resources.ErrorFailedUnpack}: {Environment.NewLine + ex.ToString()}");
		}
		return null;
	}

	public static string GetExtractFolder()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
		string path = "VideoOS";
		string randomString = GetRandomString(10);
		return Path.Combine(folderPath, path, randomString);
	}

	private static string GetRandomString(int length)
	{
		int maxValue = alphanum.Length;
		Random random = new Random();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append(alphanum[random.Next(maxValue)]);
		}
		return stringBuilder.ToString();
	}
}
