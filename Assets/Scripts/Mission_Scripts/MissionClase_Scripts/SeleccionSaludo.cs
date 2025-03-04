using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionSaludo : Seleccion
{
    [Header("Selección Saludo")]
    [SerializeField] Button buttonSaludar;
    [SerializeField] Button buttonSentarse;  
  
    [SerializeField]List<GameObject> npcsSaludo;

    //Delegados 
    public event Action<string> OnInfoUpDate;
    public event Action<bool> OnEndSaludo;

    bool saludoAlEntrar;

    private void Start()
    {
        buttonSaludar.onClick.AddListener(ButtonSaludar);
        buttonSentarse.onClick.AddListener(ButtonSentarse);
    }

    public override void Restart()
    {
        textoSeleccion.text = "¿Qué quieres hacer?";
        buttonSentarse.gameObject.SetActive(true);
        buttonSaludar.gameObject.SetActive(true);
        keywordRecognizer.SetActive(false);
        saludoAlEntrar = false;
        base.Restart();
    }

    private void OnEnable()
    {
        KeyWordRecognizer.OnSaludo += Saludado;
    }
    private void OnDisable()
    {
        KeyWordRecognizer.OnSaludo -= Saludado;
    }
    public void ActivarSeleccionSaludo()
    {
        this.gameObject.SetActive(true);
        textoSeleccionGO.SetActive(true);
    }


    //SALUDAR O SENTARSE METHODS
    void ButtonSaludar()
    {
        saludoAlEntrar = true;

        OnInfoUpDate?.Invoke("Di 'Hola' alto y claro para que te oiga todo el mundo, repítelo hasta que te escuchen");
        
        textoSeleccion.text = "Para saludar di: 'Hola' ";
        buttonSentarse.gameObject.SetActive(false);
        buttonSaludar.gameObject.SetActive(false);
        //actualUI = textoSeleccionGO;

        keywordRecognizer.SetActive(true);
        
    }

    void ButtonSentarse()
    {
        saludoAlEntrar = false;

        textoSeleccionGO.SetActive(false);
        this.gameObject.SetActive(false);
       
        foreach (GameObject npc in npcsSaludo)
        {
            npc.GetComponent<Animator>().SetBool("EmpezarClase", true);
        }
       

        OnEndSaludo?.Invoke(saludoAlEntrar);
    }


    //SALUDAR KEYWORD RECOGNIZER
    void Saludado()
    {
        textoSeleccionGO.SetActive(false);
        keywordRecognizer.SetActive(false);

        foreach (GameObject npc in npcsSaludo)
        {
            npc.GetComponent<AudioSource>().Play();
            npc.GetComponent<Animator>().SetBool("Saludar", true);
        }

        OnEndSaludo?.Invoke(saludoAlEntrar);

    }
}
