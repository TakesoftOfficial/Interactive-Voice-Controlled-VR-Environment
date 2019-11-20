using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;
using System.IO;
using System.Xml;

public class TTS : MonoBehaviour
{
    private SpVoice voice;

    string loadXMLStandalone(string fileName)
    {

        string path = Path.Combine("Resources", fileName);
        path = Path.Combine(Application.dataPath, path);
        Debug.Log("Path:  " + path);
        StreamReader streamReader = new StreamReader(path);
        string streamString = streamReader.ReadToEnd();
        Debug.Log("STREAM XML STRING: " + streamString);
        return streamString;
    }
    ///

    // Start is called before the first frame update
    void Start()
    {
        voice = new SpVoice();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
          

            voice.Volume = 100; // Volume (no xml)
            voice.Rate = 0;  //   Rate (no xml)



            voice.Speak("Hello World");
            voice.Speak("It's very hot down here");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            voice.Pause();

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            voice.Resume();
        }

        //TEST PER ANDROID
        /*	if (Input.GetTouch)
		{

			voice.Resume();
		}*/


    }
}

