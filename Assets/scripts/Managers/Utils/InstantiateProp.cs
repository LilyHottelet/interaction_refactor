using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateProp : ManagerEvents {

    private GameObject clone,inventorySprite;
    public GameObject propSpawn;
    public Transform propParent;

    private void OnEnable()
    {
        ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
    }

    private void OnDisable()
    {
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteracting;
    }

    private void OnStartedPropInteracting(GameObject go,Props prop)
    {

        switch (prop.type)
        {
            case Props.TypeOfProp.Inspect:
//                go.GetComponent<MeshRenderer>().enabled = false;
//                LookAtProp(prop.modelToDisplay,propParent);
                break;
        }
    }

    private void OnFinishedPropInteracting(GameObject go,Props prop)
    {
        switch (prop.type)
        {
            case Props.TypeOfProp.Inspect:
                go.GetComponent<MeshRenderer>().enabled = true;
                Destroy(clone);
                if (prop.isCollectable)
                {
                    KillPropGO(go);
                }
                break;
        }
    }
    
    private void KillPropGO(GameObject go)
    {
        go.GetComponent<MeshRenderer>().enabled = false;
        go.GetComponent<BoxCollider>().enabled = false;
    }
    
    private void LookAtProp (GameObject prefab,Transform parent)
    {
        if (propParent.childCount == 0)
        {
            clone = Instantiate (prefab, propSpawn.transform.position, prefab.transform.rotation, parent);
            clone.layer = 8;
            clone.transform.localScale = new Vector3 (2.5f,2.5f,2.5f);
        }		
    }
}