using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Room", menuName = "Custom/Room")]
public class Room : ScriptableObject
{
	public string sceneName;
	public Transform spawnPoint;
	public List<RoomItem> roomItemsList = new List<RoomItem>();

}

[System.Serializable]
public class RoomItem
{
    
	public bool isEnabled;
	public string _name;

	public RoomItem(bool enabled, string name)
	{
		isEnabled = enabled;
		_name = name;


	}
}