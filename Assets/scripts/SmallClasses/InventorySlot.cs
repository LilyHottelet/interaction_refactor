using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [HideInInspector]
    public Props objectContained ;
    [HideInInspector]
    public bool isEmpty = true;
    [HideInInspector]
    public string nameOfObject;
	
    public Transform position;
    public Image spriteSpot;


    public InventorySlot(Props obj,Transform pos,string name)
    {
        objectContained = obj;
        isEmpty = true;
        position = pos; 
        nameOfObject = name;

    }

}