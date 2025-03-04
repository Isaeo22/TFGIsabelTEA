using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    int step = 0;
   
    [SerializeField] Button buttonNext;

    [SerializeField] GameObject buttonsSiNo;
    [SerializeField] Button buttonSi;
    [SerializeField] Button buttonNo;

    [SerializeField] GameObject buttonSiNoSeguro;
    [SerializeField] Button buttonSiSeguro;
    [SerializeField] Button buttonNoSeguro;

    [SerializeField] GameObject instructions;
    [SerializeField] ButtonsManager buttonsManager;

 
    [SerializeField] GameObject openAIWolfieGO;
    ManagerOpenAIWolfie openAIWolfie;

    [SerializeField] Instructions instructionsController;


    [SerializeField] DeviceManager deviceController;

    public void Start()
    {
        buttonNext.onClick.AddListener(ButtonNext);
        instructionsController.OnEndInstructions +=EndTutorial;
        buttonSi.onClick.AddListener(ButtonSiTutorial);
        buttonNo.onClick.AddListener(ButtonNoTutorial);
        
        buttonSiSeguro.onClick.AddListener(ButtonSiSeguro);
        buttonNoSeguro.onClick.AddListener(ButtonNoSeguro);
    }
    public void StartTutorial()
    {
        openAIWolfieGO.SetActive(true);
        openAIWolfie = openAIWolfieGO.GetComponent<ManagerOpenAIWolfie>();
        InputManager.lookEnable = true;
        
        step = 0;
        openAIWolfie.NextTutorial(step);
        buttonNext.gameObject.SetActive(true);
    }
    
    void ButtonNext()
    {
        step = 1;
        openAIWolfie.NextTutorial(step);
        buttonNext.gameObject.SetActive(false);
        buttonsSiNo.SetActive(true);
    }

    void ButtonSiTutorial()
    {
        buttonsSiNo.SetActive(false);
        instructionsController.StartInstructions();
        InputManager.lookEnable = false;
        buttonsManager.GetChildSelectables();
    }
    void ButtonNoTutorial()
    {
        buttonsSiNo.SetActive(false);
        step = 2;
        openAIWolfie.NextTutorial(step);
        buttonSiNoSeguro.SetActive(true);
    }

    void ButtonSiSeguro()
    {
        EndTutorial();
    }

    void ButtonNoSeguro()
    {
        buttonSiNoSeguro.SetActive(false);
        ButtonNext();
    }
  

 
    void EndTutorial()
    {
        step = 3;
        buttonSiNoSeguro.SetActive(false);
        buttonSiNoSeguro.SetActive(false);
        openAIWolfie.NextTutorial(step);
        InputManager.lookEnable = true;
        Invoke("SelectDevice", 3f);
    }

    void SelectDevice()
    {    
       
        openAIWolfieGO.SetActive(false);
        deviceController.StartDevice();
    }
    
}

