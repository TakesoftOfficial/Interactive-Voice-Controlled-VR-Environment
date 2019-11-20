using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineMulti : MonoBehaviour
{

    //public string[] command;
    public GameObject obj;
    private Animator anim;

    public string MethodName;
    public bool istrue;

    public void Start()
    {
        obj = this.gameObject;
        anim = obj.GetComponent<Animator>();
    }
}
