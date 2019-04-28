using System.Collections.Generic;

namespace GameLab
{
	public partial interface IManager
	{
		void SaveGame(Dictionary<string, object> saveData);
		void LoadGame(Dictionary<string, object> saveData);
	}
}
