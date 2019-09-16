using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
   
    /*
     * Things that need to be stored :
     * 1) Player Position
     * 2) Current Phase
     * 3) Found Clues => Although this is registered in the actual data of the object itself?
     */

    public float[] playerPosition;
    public float[] playerRotation;
    public int currentPhaseID;


    public PlayerData(Transform playerPos, int currentPhaseId)
    {
        playerPosition = new float[3];
        playerPosition[0] = playerPos.position.x;
        playerPosition[1] = playerPos.position.y;
        playerPosition[2] = playerPos.position.z;
        playerRotation = new float[3];
        playerPosition[0] = playerPos.eulerAngles.x;
        playerPosition[1] = playerPos.eulerAngles.y;
        playerPosition[2] = playerPos.eulerAngles.z;
        currentPhaseID = currentPhaseId;

    }

}