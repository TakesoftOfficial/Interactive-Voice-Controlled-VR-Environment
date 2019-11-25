using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRoutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddRoutines(GameObject[] objList, Dictionary<string, ActionInvoker> actions)
    {
        for (int i = 0; i < objList.Length; i++) // Cycle through "Smart" objects
        {
            for (int j = 0; j < objList[i].transform.childCount; j++) //Cycle through child objects of "smart" object
            {
                GameObject childObj = objList[i].transform.GetChild(j).gameObject; //set a child to variable

                if (childObj.GetComponent<Routine>() == true) //Check if object has a routine script
                {
                    for (int k = 0; k < childObj.GetComponent<Routine>().command.Length; k++) // Cycle through commands
                    {
                        //Add command to dictionary
                        ActionInvoker ai = new ActionInvoker(objList[i].GetComponent<Animator>(), childObj.GetComponent<Routine>().MethodName, childObj.GetComponent<Routine>().istrue);
                        actions.Add(childObj.GetComponent<Routine>().command[k], ai);
                    }
                }
            }
        }
        printActions(actions);
    }


    void printActions(Dictionary<string, ActionInvoker> actions)
    {
        foreach (KeyValuePair<string, ActionInvoker> s in actions) //Print All Commands to console
        {
            print(s.Key);
        }
    }
}
