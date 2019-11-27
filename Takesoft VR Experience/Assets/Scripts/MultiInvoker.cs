using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInvoker : MonoBehaviour
{

    string[] command;
    string[] actionsArray;
    private Dictionary<string, ActionInvoker> actions = new Dictionary<string, ActionInvoker>();

    public void Invoke()
    {
        
    }

}
