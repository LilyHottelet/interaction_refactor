using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;

public class UIManagerVide : ManagerEvents
{

//Just making a comment to test the git 
    public GameObject npcUi;
    public GameObject playerUI;
    public Text npcText;
    public Text[] playerTextChoices;
    
    private void Start()
    {
        npcUi.SetActive(false);
        playerUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!VD.isActive)
            {
                Begin();
            }
            else
            {
                VD.Next();
            }
        }
    }

    private void Begin()
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        VD.BeginDialogue(GetComponent<VIDE_Assign>());
        Debug.Log("ttt");
    }

    private void UpdateUI(VD.NodeData data)
    {
        //What happens when we change node; 
        //First, we check whether the node is player or NPC
        npcUi.SetActive(false);
        playerUI.SetActive(false);
        if (data.isPlayer)
        {
            playerUI.SetActive(true);
            for (int i = 0; i < playerTextChoices.Length; i++)
            {
                if (i < data.comments.Length)
                {
                    playerTextChoices[i].text = data.comments[i];
                    playerTextChoices[i].transform.parent.gameObject.SetActive(true);
                }else
                {
                    playerTextChoices[i].transform.parent.gameObject.SetActive(false);
                }
            }

            playerTextChoices[0].transform.parent.GetComponent<Button>().Select();
        }
        else
        {
            npcUi.SetActive(true);
            npcText.text = data.comments[data.commentIndex]; //=> Setting the UI text to the current text from the node
            //"it will continue to do so until we reach the last one and move on to the next node."

        }
    }

    private void End(VD.NodeData data)
    {
        npcUi.SetActive(false);
        playerUI.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
        //What happends when we reach the end of dialogue;
    }

    private void OnDisable() //=> This is in case the script gets disabled by mistake during runtime
    {
        if (npcUi!=null)
        {
            End(null);
        }
    }

    public void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice;
        if (Input.GetMouseButtonUp(0))
            VD.Next(); 
    }
}
