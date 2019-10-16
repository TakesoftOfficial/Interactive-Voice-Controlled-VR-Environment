using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routine : MonoBehaviour

{ 
    public string[] command;
    private GameObject obj;
    public Animator anim;
    public Component action;
    public string MethodName;

    public void Start()
    {
        obj = this.gameObject;
       
    }

}
