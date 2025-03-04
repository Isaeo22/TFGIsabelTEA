using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Clase padre de todas las misiones
/// Maneja el UI y algunas variables para el JSON
/// </summary>
public abstract class MissionFather : MonoBehaviour
{

    [Header("Mission Settings")]
    public string missionName;//Nombre de la misión
    
    [Header("Instrucciones")]
    [SerializeField] protected GameObject instructions;
    [SerializeField] protected GameObject instruccionsInGame;
    [SerializeField] protected TextMeshProUGUI textInstrucciones;
    [SerializeField] protected Button buttonEmpezar;
    [SerializeField] protected Button buttonInfo;
    [SerializeField] protected Button buttonListo;
 

    [Header("Stats")]
    [SerializeField] protected GameObject statsGO;
    [SerializeField] protected Stats stats;
    [SerializeField] protected Button buttonAcabarMision;
    [SerializeField] protected Sprite spriteCheck;
    [SerializeField] protected Sprite spriteUnchecked;


    //Variables JSON   
    int numTotalWords = 0;
    int numMessages=0;
    int numTimesClickedInfo=0;
    [SerializeField] protected Crono cronometer;

    //Eyetracking
    [SerializeField] protected EyetrackerObject eyetrackerObject;

    [SerializeField]protected JsonManager json;
    [SerializeField]public bool OnVr;
    //Start

    public virtual void StartMission(){
        

        //Initialize Buttons BEGINING
        buttonEmpezar.onClick.AddListener(ButtonEmpezar);
        buttonInfo.onClick.AddListener(ButtonInfo);
        buttonListo.onClick.AddListener(ButtonListo);
        buttonAcabarMision.onClick.AddListener(ButtonAcabarMision);
        //Initialize Buttons END

        instructions.SetActive(true);
        instruccionsInGame.SetActive(false);
        buttonInfo.gameObject.SetActive(false);
       
    }

    //Virtual Methods BEGINING

    protected virtual void ButtonListo() 
    {        
        instruccionsInGame.SetActive(false);
        buttonInfo.gameObject.SetActive(true);
        Time.timeScale = 1;
        AudioListener.pause = false;
        buttonInfo.enabled = true;
        cronometer.StartStopwatch();
    }

    protected virtual void ButtonInfo()
    {
        instruccionsInGame.SetActive(true);
        buttonInfo.gameObject.SetActive(false);
        numTimesClickedInfo++;
        EventSystem.current.SetSelectedGameObject(buttonListo.gameObject);
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    protected virtual void ButtonEmpezar()
    {
        instructions.SetActive(false);
        AfterInstructions(); 
    }

    protected virtual void AfterInstructions(){}

    //Virtual Methods END

    //Protected Methods BEGINING
    protected void ChangeInstructions(string instruction)
    {
        if (instruction != null)
        {
            textInstrucciones.text = instruction;
        }
    }
  

    protected bool CompareSentences(string message,string word)
    {
        return message.IndexOf(word, System.StringComparison.OrdinalIgnoreCase) != -1;
    }
    protected void CheckWord(int wordIndex,List<bool> list)
    {
        list[wordIndex] = true;
    }

    protected void CreateStats(List<Image> s,List<bool> listcW)
    {
        for (int i = 0; i < s.Count; i++)
        {
            if (listcW[i])
            {
                s[i].color = Color.white;
                s[i].sprite = spriteCheck;
            }
        }
    }

    protected void ShowStats()
    {
        statsGO.SetActive(true);
    }

    protected void InitializeCheckWords(int capacity, List<bool> listcW)
    {
        for(int i=0;i<capacity;i++)
        {
            listcW.Add(false);
        }
    }
    protected void ButtonAcabarMision()
    {
        GameManager.Instance.SalirMission();
    }

    //Protected Methods END

    //METODOS JSON
    public void AddNumWordsMessage(string m)
    {
        string[]palabras=m.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int cantidadPalabras = palabras.Length;

         numTotalWords+=cantidadPalabras;
    }

    public float GetAverageNumberWordsPerMessage()
    {
        return (int)(numTotalWords / numMessages);
    }

    protected void ReestartJson()
    {
        numTotalWords = 0;
        numMessages = 0;
        numTimesClickedInfo = 0;
    }

    public void AddNumMessages()
    {
        numMessages++;
    }

    public int GetNumMessages()
    {
        return numMessages;
    }

    public int GetNumClickInfo()
    {
        return numTimesClickedInfo;
    }

    public void StartEyeTrackerTime()
    {
        eyetrackerObject.PlayCrono();
    }

    public void StopEyeTrackerTime()
    {
        eyetrackerObject.PauseCrono();
    }

    public float GetTotalTime()
    {
        return cronometer.GetElapsedTime();
    }

    public float GetTotalEyetrackingTime()
    {
        return eyetrackerObject.GetTotalEyetrackingTime();
    }

    public void ReestartEyetracker()
    {
        eyetrackerObject.Reestart();
    }
    //METODOS JSON
}
