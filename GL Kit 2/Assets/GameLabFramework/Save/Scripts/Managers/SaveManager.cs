using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;

namespace GameLab
{
	[DisallowMultipleComponent]
	public class SaveManager : Manager<SaveManager>
	{
		public const string SaveFolderName = "Save";

		private List<SaveSlot> saveSlots = new List<SaveSlot>();
		public IReadOnlyList<SaveSlot> SaveSlots => saveSlots.AsReadOnly();

		private FileInfo saveSlotsFileInfo = null;

		private HashSet<IManager> managers = new HashSet<IManager>();

		protected override void Awake()
		{
			base.Awake();

			saveSlotsFileInfo = new FileInfo(Path.Combine(Application.persistentDataPath, SaveFolderName, "SaveSlots"));

			LoadSaveSlotsFromDisk();

			if(saveSlots.Count == 0)
			{
				CreateNewSaveSlot("Save Slot 1");
			}
		}

		/// <summary>
		/// Registers a manager to the list of managers that will be saved.
		/// This is normally called automatically in the base manager class on Awake.
		/// </summary>
		/// <param name="manager">The manager to add</param>
		public void AddManager(IManager manager)
		{
			if(managers.Contains(manager))
			{
				return;
			}

			managers.Add(manager);
		}

		/// <summary>
		/// Removes a manager from the list of managers that will be saved.
		/// This is normally called automatically in the base manager class on Destroy.
		/// </summary>
		/// <param name="manager"></param>
		public void RemoveManager(IManager manager)
		{
			if(!managers.Contains(manager))
			{
				return;
			}

			managers.Remove(manager);
		}

		/// <summary>
		/// Creates a new save slot with the provided name.
		/// This is a utility method to hook up creating a new slot directly to UnityEvents.
		/// </summary>
		/// <param name="slotName">The name of the slot</param>
		public void NewSaveSlot(string slotName) => CreateNewSaveSlot(slotName);

		/// <summary>
		/// Creates a new save slot with the provided name and returns the new slot object.
		/// </summary>
		/// <param name="slotName">The name of the slot</param>
		/// <returns>The newly created save slot or an already existing save slot if one exists with the provided name</returns>
		public SaveSlot CreateNewSaveSlot(string slotName)
		{
			SaveSlot slot = GetSaveSlot(slotName);

			if(slot != null)
			{
				return slot;
			}

			slot = new SaveSlot(slotName);
			saveSlots.Add(slot);

			WriteSaveSlotsToDisk();

			return slot;
		}

		/// <summary>
		/// Deletes a save slot based on a name if it exists.
		/// </summary>
		/// <param name="slotName">The name of the slot to delete</param>
		public void DeleteSaveSlot(string slotName) => DeleteSaveSlot(GetSaveSlot(slotName));

		/// <summary>
		/// Deletes a save slot if it exists.
		/// </summary>
		/// <param name="slot">The slot to delete</param>
		public void DeleteSaveSlot(SaveSlot slot)
		{
			if(slot == null || !saveSlots.Contains(slot))
			{
				return;
			}

			saveSlots.Remove(slot);

			WriteSaveSlotsToDisk();
		}

		/// <summary>
		/// Finds a save slot object based on a slot name.
		/// </summary>
		/// <param name="slotName">The name of the slot to use to find a slot object</param>
		/// <returns>Found save slot object or null if no save slots exist with the provided name</returns>
		public SaveSlot GetSaveSlot(string slotName) => saveSlots.FirstOrDefault(saveSlot => saveSlot.SlotName == slotName);

		/// <summary>
		/// Saves all the managers to the first save slot.
		/// </summary>
		public void Save()
		{
			Save(SaveSlots[0]);
		}

		/// <summary>
		/// Loads all the managers from the first save slot.
		/// </summary>
		public void Load()
		{
			Load(SaveSlots[0]);
		}

		/// <summary>
		/// Saves all the managers to <paramref name="slot"/>.
		/// </summary>
		/// <param name="slot">The slot to save the managers to</param>
		public void Save(SaveSlot slot)
		{
			Dictionary<string, Dictionary<string, object>> saveData = new Dictionary<string, Dictionary<string, object>>();

			foreach(IManager manager in managers)
			{
				Dictionary<string, object> managerSaveData = new Dictionary<string, object>();
				manager.SaveGame(managerSaveData);

				if(managerSaveData.Count == 0)
				{
					continue;
				}

				saveData.Add(manager.GetType().Name, managerSaveData);
			}

			slot.Save(saveData);
		}

		/// <summary>
		/// Loads all the managers from <paramref name="slot"/>.
		/// </summary>
		/// <param name="slot">The slot to load the managers from</param>
		public void Load(SaveSlot slot)
		{
			Dictionary<string, Dictionary<string, object>> saveData = slot.Load();

			if(saveData == null)
			{
				return;
			}

			foreach(IManager manager in managers)
			{
				if(!saveData.ContainsKey(manager.GetType().Name))
				{
					continue;
				}

				Dictionary<string, object> saveDataManager = saveData[manager.GetType().Name];
				manager.LoadGame(saveDataManager);
			}
		}

		private void WriteSaveSlotsToDisk()
		{
			string saveSlotDataJson = JsonConvert.SerializeObject(saveSlots, Formatting.Indented);
			saveSlotsFileInfo.WriteAllText(saveSlotDataJson);
		}

		private void LoadSaveSlotsFromDisk()
		{
			string saveSlotDataJson = saveSlotsFileInfo.ReadAllText();

			if(saveSlotDataJson == string.Empty)
			{
				return;
			}

			saveSlots = JsonConvert.DeserializeObject<List<SaveSlot>>(saveSlotDataJson);
		}
	}
}