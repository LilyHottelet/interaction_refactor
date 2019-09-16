using System.Collections;
using I18N.Common;
using UnityEngine;

public class NextActionHandler : ManagerEvents {
    
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

    private void OnStartedPropInteracting(GameObject go, Props prop)
    {
        if (prop.hasNextAction)
        {
            go.GetComponent<BoxCollider>().enabled = false;
            FindInteractable(go,true);

        }
    }

    private void OnFinishedPropInteracting(GameObject go, Props prop)
    {
        if (prop.hasNextAction)
        {
            StartCoroutine(DelayColliderEnabling(go));
            FindInteractable(go,false);
        }

        if (prop.isNestedAction)
        {
            go.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void FindInteractable(GameObject go,bool isEnabling)
    {
         
        for(int i = 0; i < go.transform.childCount; i++)
        {
            Transform obj = go.transform.GetChild(i);
            if (obj.GetComponent<PropComponent>() && obj.GetComponent<BoxCollider>())
            {
                obj.GetComponent<BoxCollider>().enabled = isEnabling;
            }
        }
    }

    private IEnumerator DelayColliderEnabling(GameObject go)
    {
        yield return new WaitForSeconds(1.7f);
        go.GetComponent<BoxCollider>().enabled = true;
    }
}