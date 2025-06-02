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
    [SerializeField] protected Button buttonAcabarMision;
    [SerializeField] protected Sprite spriteCheck;
    [SerializeField] protected Sprite spriteUnchecked;


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
       
    }

    protected virtual void ButtonInfo()
    {
        instruccionsInGame.SetActive(true);
        buttonInfo.gameObject.SetActive(false);
     
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

    protected void ShowStats()
    {
        statsGO.SetActive(true);
    }


    protected void ButtonAcabarMision()
    {
        GameManager.Instance.SalirMission();
    }

    //Protected Methods END

  
}
