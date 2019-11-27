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
    private Dictionary<string, TTSInvoker> TTS = new Dictionary<string, TTSInvoker>();
    private Dictionary<string, Action> WakeUpWords = new Dictionary<string, Action>();
    private PhraseRecognizer phraseRecognizer;
    bool isAwake = false;
    public string WakeUpWord = "Echo";
    public GameObject[] Objects;
    public GameObject[] voiceOnly;
    private List<Routine> routineList;
    private bool voiceCommand;
    Coroutine timer;

    public AudioClip OnSound;
    public AudioClip OffSound;
    private AudioSource SFX;

    //public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    // Text to Speech
    private SpVoice voice;
    ISpeechObjectTokens voices;

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

        voices = voice.GetVoices("", "");

        
        voice.Voice = voices.Item(1);

        WakeUpWords.Add(WakeUpWord, WakeUp);
        WakeUpWords.Add("Siri", WakeUp);
        WakeUpWords.Add("Alexa", WakeUp);
        WakeUpWords.Add("Hey Google", WakeUp);
        WakeUpWords.Add("OK Google", WakeUp);
        WakeUpWords.Add("Jarvis", WakeUp);

        SFX = this.GetComponent<AudioSource>();
        SFX.loop = false;

        AddRoutines(Objects, actions);
        AddTTSRoutines(voiceOnly, TTS);

        WakeUpRecogniser = new KeywordRecognizer(WakeUpWords.Keys.ToArray());
        WakeUpRecogniser.OnPhraseRecognized += RecognizedWakeUpWord;
        WakeUpRecogniser.Start();
        isAwake = false;

        string[] keywordArray = new String[actions.Keys.Count + TTS.Keys.Count];
        string[] actionArray = actions.Keys.ToArray();
        string[] TTSArray = TTS.Keys.ToArray();

        for (int i = 0; i < actionArray.Length; i++)
        {
            keywordArray[i] = actionArray[i];
        }

        for (int i = 0; i < TTSArray.Length; i++)
        {
            keywordArray[actionArray.Length + i] = TTSArray[i];
        }

        for (int i = 0; i < keywordArray.Length; i++)
        {
            print(keywordArray[i]);
        }

        keywordRecognizer = new KeywordRecognizer(keywordArray);
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

        if (speech.text == "Jarvis")
        {
            voice.Voice = voices.Item(0);
        }
        else
        {
            voice.Voice = voices.Item(1);
        }
        WakeUpWords[speech.text].Invoke();
    }
    private void RecognizedCommand(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        if (isAwake == true)
        {

            try
            {
                actions[speech.text].Invoke();
            }
            catch (KeyNotFoundException)
            {
                TTS[speech.text].Invoke();
            }


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

    void AddTTSRoutines(GameObject[] voiceOnly, Dictionary<string, TTSInvoker> TTS)
    {
        for (int i = 0; i < voiceOnly.Length; i++) // Cycle through "Smart" objects
        {
            for (int j = 0; j < voiceOnly[i].transform.childCount; j++) //Cycle through child objects of "smart" object
            {
                GameObject childObj = voiceOnly[i].transform.GetChild(j).gameObject; //set a child to variable

                if (childObj.GetComponent<TTSRoutine>() == true) //Check if object has a routine script
                {
                    for (int k = 0; k < childObj.GetComponent<TTSRoutine>().command.Length; k++) // Cycle through commands
                    {
                        //Add command to dictionary
                        TTSInvoker ti = new TTSInvoker(voice, childObj.GetComponent<TTSRoutine>().textToSpeech);
                        TTS.Add(childObj.GetComponent<TTSRoutine>().command[k], ti);
                    }
                }
            }
        }
        printTTS(TTS);
    }


    void printActions(Dictionary<string, ActionInvoker> TTS)
    {
        foreach (KeyValuePair<string, ActionInvoker> s in TTS) //Print All Commands to console
        {
            print(s.Key);
        }
    }

    void printTTS(Dictionary<string, TTSInvoker> actions)
    {
        foreach (KeyValuePair<string, TTSInvoker> s in actions) //Print All Commands to console
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