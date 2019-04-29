using System.Collections.Generic;

namespace GameLab
{
	public abstract partial class Manager<T>
	{
		protected override void Awake()
		{
			base.Awake();

			SaveManager.Instance.AddManager(this);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			if(SaveManager.IsInitialized)
			{
				SaveManager.Instance.RemoveManager(this);
			}
		}

		/// <summary>
		/// Override this method to provide data to the save manager to save.
		/// </summary>
		/// <param name="saveData">Dictionary to modify and put the data to save in</param>
		public virtual void SaveGame(Dictionary<string, object> saveData) {}

		/// <summary>
		/// Override this method to load data from the save manager.
		/// </summary>
		/// <param name="saveData">Dictionary that contains the data saved using <see cref="SaveGame(Dictionary{string, object})"/></param>
		public virtual void LoadGame(Dictionary<string, object> saveData) {}
	}
}
