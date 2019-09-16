using UnityEngine;

public class MoralChoice : ManagerEvents
{

	public Room paxtonScene;
	public Room haroldScene;
	
	public void PickPaxton()
	{
		Debug.Log("you picked good ol pax");
		OnMustLoadScene(paxtonScene);
	}

	public void PickHarold()
	{
		Debug.Log("you picked good ol harold");
		OnMustLoadScene(haroldScene);
	}
}
