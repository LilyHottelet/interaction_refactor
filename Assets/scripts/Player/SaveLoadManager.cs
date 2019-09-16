using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadManager {

	public static void SavePlayerData(Transform playerTransform,int currentPhaseID)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath+"/player.sav",FileMode.Create);
		PlayerData playerData = new PlayerData(playerTransform, currentPhaseID);
		formatter.Serialize(stream,playerData);
		stream.Close();
	}

	public static PlayerData LoadPlayerData()
	{
		if (File.Exists(Application.persistentDataPath+"/player.sav"))
		{
			BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath+"/player.sav",FileMode.Open);
			PlayerData data = formatter.Deserialize(stream) as PlayerData;
			stream.Close();
			return data;
		}
		else
		{
			Debug.Log("Error, no savefile");
			return null;
		}
	}
	
	
}
