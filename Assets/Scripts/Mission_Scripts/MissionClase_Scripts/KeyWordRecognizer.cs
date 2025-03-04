using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeyWordRecognizer : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    List<string> wordToAction=new List<string>();

    public delegate void SaludoDelegate();
    public static event SaludoDelegate OnSaludo;


    public delegate void PreguntaDelegate();
    public static event PreguntaDelegate OnPreguntar;

    // Start is called before the first frame update
    void Start()
    {
        wordToAction.Add("Hola");
        wordToAction.Add("Perdón, está ocupado");

        keywordRecognizer = new KeywordRecognizer(wordToAction.ToArray());

        keywordRecognizer.OnPhraseRecognized += WordRecognized;
        keywordRecognizer.Start();
    }

    void OnDisable()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
        }
    }

    void OnEnable()
    {
        if (keywordRecognizer != null && !keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Start();
        }
    }

    private void WordRecognized(PhraseRecognizedEventArgs word)
    {
        Debug.Log(word.text);
        CheckWord(word.text);
    }


    void CheckWord(string w) {

        if (w == "Hola")
        {
            OnSaludo();
        }else if (w == "Perdón, está ocupado")
        {
            OnPreguntar();
        }

    }

 }
