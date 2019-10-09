using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;


public class MicInput : MonoBehaviour
{
   
    private AudioSource audioSource;
    public string microphone;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start("Meteor", true, 10, 44100);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
