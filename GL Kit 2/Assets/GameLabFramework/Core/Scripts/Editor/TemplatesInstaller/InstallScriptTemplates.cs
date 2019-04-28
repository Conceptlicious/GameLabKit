using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameLab;
using System.IO;
using System.Linq;

public class InstallScriptTemplates : AssetPostprocessor
{
	private const string customTemplatesFolderName = "CustomScriptTemplates";

	private static readonly string unityEditorScriptsTemplatesPath = Path.Combine(Path.GetDirectoryName(EditorApplication.applicationPath), "Data", "Resources", "ScriptTemplates");
	private static readonly string operationSaveDataPath = Path.Combine(unityEditorScriptsTemplatesPath, "Installed Templates");

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		InstallTemplates();
	}

	[MenuItem("GameLab/Tools/Re-Install Custom Script Templates")]
	private static void ReinstallTemplates()
	{
		File.Delete(operationSaveDataPath);
		InstallTemplates();
	}

	[InitializeOnLoadMethod]
	[MenuItem("GameLab/Tools/Install Custom Script Templates", priority = 10)]
	private static void InstallTemplates()
	{	
		string[] allProjectDirectories = Directory.GetDirectories(Directory.GetCurrentDirectory(), customTemplatesFolderName, SearchOption.AllDirectories);
		string[] customTemplatesDirectoriers = (from directory in allProjectDirectories
											   where Path.GetFileName(directory) == customTemplatesFolderName select directory).ToArray();

		if(customTemplatesDirectoriers.Length == 0)
		{
			Debug.LogError($"No scripts templates directories found. Make sure that you have a directory named {customTemplatesFolderName} to be able to install custom unity script templates");
			return;
		}

		List<string> installedTemplates = LoadPreviousOperation();
		int installCount = 0;

		foreach(string customTemplatesDirectory in customTemplatesDirectoriers)
		{
			string[] templatesToInstall = Directory.GetFiles(customTemplatesDirectory);
			templatesToInstall = (from template in templatesToInstall
								  where !string.IsNullOrEmpty(template) && Path.GetExtension(template).ToLower() == ".txt" && !installedTemplates.Any(installedTemplate => installedTemplate == Path.GetFileName(template))
								  select template).ToArray();

			if(templatesToInstall.Length == 0)
			{
				continue;
			}

			for(int i = 0; i < templatesToInstall.Length; ++i)
			{
				string scriptTemplatePath = templatesToInstall[i];
				string scriptTemplateName = Path.GetFileName(scriptTemplatePath);

				File.Copy(scriptTemplatePath, Path.Combine(unityEditorScriptsTemplatesPath, scriptTemplateName), true);

				installedTemplates.Add(scriptTemplateName);
				++installCount;
			}
		}

		if(installCount == 0 || installedTemplates.Count == 0)
		{
			return;
		}

		SaveOperation(installedTemplates);

		string successMessage = $"Successfully installed {installCount} new custom Unity C# Script Templates. Please restart Unity for changes to take effect.";
		Debug.Log(successMessage);

		if(EditorUtility.DisplayDialog("Custom Script Templates", successMessage, "Restart", "Not Now"))
		{
			RestartUnityEditor();
		}
	}

	private static void SaveOperation(List<string> installedTemplates)
	{
		if(File.Exists(operationSaveDataPath))
		{
			File.Delete(operationSaveDataPath);
		}

		using(FileStream fileStream = File.Create(operationSaveDataPath))
		using(StreamWriter streamWriter = new StreamWriter(fileStream))
		{
			foreach(string template in installedTemplates)
			{
				if(string.IsNullOrEmpty(template))
				{
					continue;
				}

				streamWriter.WriteLine(Path.GetFileName(template));		
			}
		}

		File.SetAttributes(operationSaveDataPath, File.GetAttributes(operationSaveDataPath) | FileAttributes.Hidden);
	}

	private static List<string> LoadPreviousOperation()
	{
		if(!File.Exists(operationSaveDataPath))
		{
			return new List<string>();
		}

		return File.ReadAllLines(operationSaveDataPath).ToList();
	}

	private static string GetCurrentProjectPath()
	{
		return Directory.GetParent(Application.dataPath).FullName;
	}

	private static void RestartUnityEditor()
	{
		EditorApplication.OpenProject(GetCurrentProjectPath());
	}
}
