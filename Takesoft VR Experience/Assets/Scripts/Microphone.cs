using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Windows.Speech;
using System;
using System.Linq;
using UnityEngine.Animations;

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


    private void Start()
    {
        
        WakeUpWords.Add(WakeUpWord, WakeUp);
        SFX = this.GetComponent<AudioSource>();
        SFX.loop = false;

        for (int i = 0; i < Objects.Length; i++)
        {
           
                for (int j = 0; j < Objects[i].transform.childCount; j++)
                {
                    GameObject childObj = Objects[i].transform.GetChild(j).gameObject;
                    foreach (string command in childObj.GetComponent<Routine>().command)
                    {
                    ActionInvoker ai = new ActionInvoker(Objects[i].GetComponent<Animator>(), childObj.GetComponent<Routine>().MethodName, childObj.GetComponent<Routine>().istrue);
                    actions.Add(command, ai);
                    }
                
                }         
        }

        foreach (KeyValuePair<string, ActionInvoker> s in actions)
        {
            print(s.Key);
        }

        WakeUpRecogniser = new KeywordRecognizer(WakeUpWords.Keys.ToArray());
        WakeUpRecogniser.OnPhraseRecognized += RecognizedSpeech;
        WakeUpRecogniser.Start();
        isAwake = false;

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech1;

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

    //private void OnGUI()
    //{
    //    if(GUILayout.Button("Start Waiting"))
    //    {
    //        StartCoroutine(DoWaitTest());
    //    }
    //}

    internal static AudioClip Start(string v1, bool v2, int v3, int v4)
    {
        throw new NotImplementedException();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);

        WakeUpWords[speech.text].Invoke();
    }
    private void RecognizedSpeech1(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        if (isAwake == true)
        {
            ActionInvoker temp = actions[speech.text];
            temp.anim.SetBool(temp.str, temp.isTrue);
            
        }
        Finish();
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