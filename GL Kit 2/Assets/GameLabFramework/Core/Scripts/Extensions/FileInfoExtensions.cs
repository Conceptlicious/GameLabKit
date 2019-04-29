using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLab
{
	public static class FileInfoExtensions
	{
		/// <summary>
		/// Creates all the directories and the file at the file info's path and writes <paramref name="contents"/> to it.
		/// </summary>
		/// <param name="contents">The contents to write to the file</param>
		public static void WriteAllText(this FileInfo fileInfo, string contents)
		{
			fileInfo.Directory.Create();
			File.WriteAllText(fileInfo.FullName, contents);
		}

		/// <summary>
		/// Reads all the contents from the file and returns them or an empty string if the file does not exist.
		/// </summary>
		/// <returns>File contents or empty string if file does not exist</returns>
		public static string ReadAllText(this FileInfo fileInfo)
		{
			if(!fileInfo.Exists)
			{
				return string.Empty;
			}

			return File.ReadAllText(fileInfo.FullName);
		}
	}
}
