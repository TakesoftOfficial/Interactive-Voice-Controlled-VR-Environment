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
    private Dictionary<string, Action> WakeUpWord = new Dictionary<string, Action>();
    private PhraseRecognizer phraseRecognizer;
    bool isAwake = false;

    private void Start()
    {
        WakeUpWord.Add("Echo", WakeUp);
        actions.Add("Forward", Forward);
        actions.Add("up", Up);
        actions.Add("down", Down);
        actions.Add("back", Back);

        WakeUpRecogniser = new KeywordRecognizer(WakeUpWord.Keys.ToArray());
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

        WakeUpWord[speech.text].Invoke();
    }
    private void RecognizedSpeech1(PhraseRecognizedEventArgs speech)
    {

        Debug.Log(speech.text);
        if (isAwake == true)
        { actions[speech.text].Invoke(); }
    }

    private void Forward()
    {
        transform.Translate(1, 0, 0);
        keywordRecognizer.Stop();
        isAwake = false;
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
        
        keywordRecognizer.Start();
        isAwake = true;
    }



}
