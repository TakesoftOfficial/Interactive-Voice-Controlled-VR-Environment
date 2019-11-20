using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineM : MonoBehaviour
{

    private GameObject[] objects;

    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            GameObject childObj = this.gameObject.transform.GetChild(i).gameObject; //set a child to variable
            objects[i] = childObj;
        }
    }

  
    
}
