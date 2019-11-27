using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;
using System.IO;
using System.Xml;


public class TTSInvoker : MonoBehaviour
{

    SpVoice voice;
    string script;
     
    public TTSInvoker(SpVoice voice, string script)
    {
        this.voice = voice;
        this.script = script;
    }

    public void Invoke()
    {
        voice.Speak(script);
    }
   
       
}
