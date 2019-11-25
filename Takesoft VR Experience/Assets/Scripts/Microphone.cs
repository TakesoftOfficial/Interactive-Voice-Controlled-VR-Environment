using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Windows.Speech;
using System;
using System.Linq;
using UnityEngine.Animations;
using SpeechLib;
using System.IO;
using System.Xml;

public class Microphone : MonoBehaviour
{
    //public float waitTime = 3;
    //WaitForSecondsRealtime waitForSeconsRealtime;
    private KeywordRecognizer keywordRecognizer;
    private KeywordRecognizer WakeUpRecogniser;
    private Dictionary<string, ActionInvoker> actions = new Dictionary<string, ActionInvoker>();
    private Dictionary<string, Action> WakeUpWords = new Dictionary<string, Action>();
    private PhraseRecognizer phraseRecognizer;
    bool isAwake = false;
    public string WakeUpWord = "Echo";
    public GameObject[] Objects;
    private List<Routine> routineList;
    private bool voiceCommand;
    Coroutine timer;

    public AudioClip OnSound;
    public AudioClip OffSound;
    private AudioSource SFX;

    //public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    // Text to Speech
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
   

    private void Start()
    {
        voice = new SpVoice();

        WakeUpWords.Add(WakeUpWord, WakeUp);
        WakeUpWords.Add("Siri", WakeUp);
        WakeUpWords.Add("Alexa", WakeUp);
        WakeUpWords.Add("Hey Google", WakeUp);
        WakeUpWords.Add("OK Google", WakeUp);
        WakeUpWords.Add("Jarvis", WakeUp);

        SFX = this.GetComponent<AudioSource>();
        SFX.loop = false;

        AddRoutines(Objects, actions);

        

        WakeUpRecogniser = new KeywordRecognizer(WakeUpWords.Keys.ToArray());
        WakeUpRecogniser.OnPhraseRecognized += RecognizedWakeUpWord;
        WakeUpRecogniser.Start();
        isAwake = false;

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedCommand;

    }


    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }

        isAwake = false;
        keywordRecognizer.Stop();
        PlayOffSound();
        print("Now Going to Sleep...");
    }



    internal static AudioClip Start(string v1, bool v2, int v3, int v4)
    {
        throw new NotImplementedException();
    }

    private void RecognizedWakeUpWord(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);

        WakeUpWords[speech.text].Invoke();
    }
    private void RecognizedCommand(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        if (isAwake == true)
        {
            ActionInvoker temp = actions[speech.text];
            temp.anim.SetBool(temp.str, temp.isTrue);
            
        }
        Finish();
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

    void AddTTSRoutine(GameObject[] objList, Dictionary<string, ActionInvoker> actions)
    {
        for (int i = 0; i < objList.Length; i++) // Cycle through "Smart" objects
        {
            for (int j = 0; j < objList[i].transform.childCount; j++) //Cycle through child objects of "smart" object
            {
                GameObject childObj = objList[i].transform.GetChild(j).gameObject; //set a child to variable

                if (childObj.GetComponent<TTSRoutine>() == true) //Check if object has a routine script
                {
                    for (int k = 0; k < childObj.GetComponent<Routine>().command.Length; k++) // Cycle through commands
                    {
                        //Add command to dictionary
                        //ActionInvoker ai = new ActionInvoker(objList[i].GetComponent<Animator>(), childObj.GetComponent<RoutineRoutine>().MethodName, childObj.GetComponent<Routine>().istrue);
                        actions.Add(childObj.GetComponent<TTSRoutine>().command[k], ai);
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

    private void WakeUp()
    {
        print("Now Listening...");
        keywordRecognizer.Start();
        isAwake = true;
        PlayOnSound();
        timer = StartCoroutine(StartCountdown());
    }

    private void Finish()
    {
        StopCoroutine(timer);
        keywordRecognizer.Stop();
        print("Action Completed");
    }


    private void PlayOnSound()
    {
        SFX.clip = OnSound;
        SFX.Play();

    }

    private void PlayOffSound()
    {
        SFX.clip = OffSound;
        SFX.Play();

    }

}