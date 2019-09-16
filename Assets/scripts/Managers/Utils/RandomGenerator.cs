using System.Collections;
using System.Collections.Generic;
using I18N.Common;
using UnityEngine;

public class RandomGenerator : ManagerEvents
{

    public static int currentRandomInt;
   
    private void OnEnable()
    {
        ManagerEvents.DetectiveRemarkStarted += OnDetectiveRemarkStart;
    }

    private void OnDisable()
    {
        ManagerEvents.DetectiveRemarkStarted += OnDetectiveRemarkStart;
    }

    private void OnDetectiveRemarkStart(Clue clue)
    {
        SetRandomInt();
    }

    private void SetRandomInt()
    {
        currentRandomInt = Random.Range(1, 11);
    }
}