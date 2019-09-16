using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dictionaries : MonoBehaviour {


   public static Dictionary<string,Clue> ClueDictionary = new Dictionary<string, Clue>();

   private void Awake()
   {
      foreach (var obj in Resources.LoadAll<Clue>("Clues"))
      {
         ClueDictionary.Add(obj.ID,obj);
      }
   }
}
