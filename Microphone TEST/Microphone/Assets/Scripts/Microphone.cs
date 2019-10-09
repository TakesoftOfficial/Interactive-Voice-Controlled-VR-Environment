using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Windows.Speech;
using System;
using System.Linq;

public class Microphone : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private KeywordRecognizer WakeUpRecogniser;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private Dictionary<string, Action> WakeUpWords = new Dictionary<string, Action>();
    private PhraseRecognizer phraseRecognizer;
    bool isAwake = false;

    public string WakeUpWord = "Echo";

    public GameObject[] Objects;

    public List<Routine> routineList;

    private void Start()
    {
        WakeUpWords.Add(WakeUpWord, WakeUp);
        //actions.Add("Forward", Forward);
        //actions.Add("up", Up);
        //actions.Add("down", Down);
        //actions.Add("back", Back);

        for (int i = 0; i < Objects.Length; i++)
        {
            foreach (string command in Objects[i].GetComponent<Routine>().command)
            {
                actions.Add(command, Forward);
            }
            
        }

        foreach (KeyValuePair<string, System.Action> s in actions)
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
            actions[speech.text].Invoke();
        }

        
    }

    private void Forward()
    {
        transform.Translate(1, 0, 0);
        keywordRecognizer.Stop();
        isAwake = false;
        print("Action Completed");
    }

    private void Back()
    {
        transform.Translate(-1, 0, 0);
        keywordRecognizer.Stop();
        isAwake = false;
        
    }

    private void Up()
    {
        transform.Translate(0, 1, 0);
        keywordRecognizer.Stop();
        isAwake = false;
    }

    private void Down()
    {
        transform.Translate(0, -1, 0);
        keywordRecognizer.Stop();
        isAwake = false;
    }

    private void WakeUp()
    {
        print("Now Listening...");
        keywordRecognizer.Start();
        isAwake = true;
    }

}
