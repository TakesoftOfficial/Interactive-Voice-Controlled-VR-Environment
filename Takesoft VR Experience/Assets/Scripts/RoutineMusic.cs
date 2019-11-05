using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] command;
    public GameObject AudioSource;
    private AudioSource audioSrc;
    public AudioClip[] music;
   
    public string MethodName;
    public bool istrue;

    public void Start()
    {
        //obj = this.gameObject;

    }

    public AudioSource getAudioSource()
    {
        return this.audioSrc;
    }
}
