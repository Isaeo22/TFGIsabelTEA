using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionPregunta : Seleccion
{
    [Header("Selecci�n Preguntar")]
    
    [SerializeField] Button buttonPreguntar;
    [SerializeField] Button buttonSentarse;
    Silla actualSilla;

    //Delegados 
    public event Action<string> OnInfoUpDate;
    public event Action <bool>OnEndPregunta;

    bool preguntoSiOcupado;
    private void Start()
    {
        buttonPreguntar.onClick.AddListener(ButtonPreguntar);
        buttonSentarse.onClick.AddListener(ButtonSentarse);
    }
    public override void Restart()
    {
        buttonPreguntar.gameObject.SetActive(true);
        buttonSentarse.gameObject.SetActive(true);
        preguntoSiOcupado = false;
        keywordRecognizer.SetActive(false);
        base.Restart();
    }
    private void OnEnable()
    {
        KeyWordRecognizer.OnPreguntar += Preguntado;
    }
    private void OnDisable()
    {
        KeyWordRecognizer.OnPreguntar -= Preguntado;
    }

    public void ActivarSeleccionPregunta(Silla s)
    {
        Debug.Log("Empieza seleccion pregunta");
        textoSeleccionGO.SetActive(true);
        actualSilla = s;
        textoSeleccion.text = "El sitio est� ocupada por " + actualSilla.item;
        this.gameObject.SetActive(true);
        //actualUI = seleccionPreguntar;
    }

    void ButtonPreguntar()
    {
        OnInfoUpDate?.Invoke("Di 'Perd�n, �est� ocupado?' al alumno m�s cercano \n Rep�telo alto y claro hasta que te escuche");

        buttonPreguntar.gameObject.SetActive(false);
        buttonSentarse.gameObject.SetActive(false);

        //actualUI = textoSeleccionGO;

        actualSilla.npc.GetComponent<AudioSource>().clip = actualSilla.clip;

        textoSeleccion.text = "Pregunta: '�Perd�n, est� ocupado?'";

        keywordRecognizer.SetActive(true);
        Debug.Log("ButtonPregguntar");
        preguntoSiOcupado = true;
    }


    void ButtonSentarse()
    {
        preguntoSiOcupado = false;

        textoSeleccion.text = "La clase empezar� pronto";

        buttonPreguntar.gameObject.SetActive(false);
        buttonSentarse.gameObject.SetActive(false);

        //actualUI = textoSeleccionGO;
        buttonInfo.gameObject.SetActive(false);

        OnEndPregunta?.Invoke(preguntoSiOcupado);

        //Invoke("EmpezarClase", 3.0f);
    }



    void Preguntado()
    {
        keywordRecognizer.SetActive(false);
        textoSeleccionGO.SetActive(false);

        actualSilla.npc.GetComponent<AudioSource>().Play();
        KeyWordRecognizer.OnPreguntar -= Preguntado;

        Invoke("CambiarItems", 3.0f);
    }


    void CambiarItems()
    {
        textoSeleccion.text = "�Bien hecho!, la clase empezar� pronto";
        buttonInfo.gameObject.SetActive(false);
       // actualUI = textoSeleccionGO;
        textoSeleccionGO.SetActive(true);
        actualSilla.row1Item.SetActive(false);
        actualSilla.row2Item.SetActive(true);

        OnEndPregunta?.Invoke(preguntoSiOcupado);
        // Invoke("EmpezarClase", 3.0f);
    }
}
