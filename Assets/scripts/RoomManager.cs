//using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomManager : MonoBehaviour
{

    public Room currentRoom;
    public GameObject[] npcsInRoom;
    public GameObject[] scenarioProps;


    private void OnDisable()
    {
        ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
        
        foreach (var npc in npcsInRoom)
        {
           CheckForNpcDisabling(npc);
        }

        if (scenarioProps != null)
        {
            foreach (var prop in scenarioProps)
            {
                CheckForPropDisabling(prop);
            }
        }
        
           
    }

    private void OnEnable()
    {
        ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
        foreach (var npc in npcsInRoom)
        {
            CheckForNpcDisabling(npc);
        }
        if (scenarioProps != null)
        {
            foreach (var prop in scenarioProps)
            {
                CheckForPropDisabling(prop);
            }
        }
    }

    private void OnDetectiveRemarkEnd(Clue clue)
    {
        if (currentRoom.sceneName == "kevin_lighting")
        {
            foreach (var npc in npcsInRoom)
            { 
                NPCComponent npcData = npc.GetComponent<NPCComponent>();
                if (npcData.requiredClue!=null)
                {
                    if (clue == npcData.requiredClue)
                    {
                        npc.SetActive(true);
                    }
                }
            }
        }
       
    }

    private void CheckForNpcDisabling(GameObject npc)
    {
        RoomItem internalNpc = currentRoom.roomItemsList.Find(item => item._name == npc.name);
        npc.SetActive(internalNpc.isEnabled);
        NPCComponent npcData = npc.GetComponent<NPCComponent>();
        if (npcData.enteringPhase != null && npcData.leavingPhase != null)
        {
            Debug.Log(PhaseManager.currentPhase.phaseIndex);
            if (PhaseManager.currentPhase.phaseIndex >= npcData.enteringPhase.phaseIndex)
            {
                npc.SetActive(true);
                internalNpc.isEnabled = true;
                //EditorUtility.SetDirty(currentRoom);
                if (PhaseManager.currentPhase.phaseIndex >= npcData.leavingPhase.phaseIndex)
                {
                    npc.SetActive(false);
                    internalNpc.isEnabled = false;
                    //EditorUtility.SetDirty(currentRoom);
                
                } 

            } 
        }
        
    }

    private void CheckForPropDisabling(GameObject prop)
    {
        RoomItem internalProp = currentRoom.roomItemsList.Find(item => item._name == prop.name);
        prop.GetComponent<BoxCollider>().enabled = internalProp.isEnabled; 
        PropComponent propData = prop.GetComponent<PropComponent>();
        if (PhaseManager.currentPhase.phaseIndex >= propData.requiredPhase.phaseIndex)
        {
            prop.GetComponent<BoxCollider>().enabled = true; 
            internalProp.isEnabled = true;
            //EditorUtility.SetDirty(currentRoom);

        }
    }
   
    
   
}