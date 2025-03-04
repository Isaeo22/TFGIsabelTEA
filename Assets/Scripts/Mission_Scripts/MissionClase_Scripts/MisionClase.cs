using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MisionClase : MissionFather
{
   
    [SerializeField] SeleccionSaludo seleccionSaludoController;
    [SerializeField] SeleccionSilla seleccionSillaController;
    [SerializeField] SeleccionPregunta seleccionPreguntaController;
    [SerializeField] ClaseExplicacion claseExplicacionController;
    [SerializeField] ClaseEjercicio claseEjercicioController;


    [SerializeField] GameObject textoSeleccionGO;

    Silla actualSilla;

    [SerializeField] TextMeshProUGUI titulo;
    [SerializeField] TextMeshProUGUI objetivo;
    [SerializeField] TextMeshProUGUI indicaciones;

    [SerializeField] GameObject pizarra;
    [SerializeField] ProfesorManager profesor;


 

    //Stats
    [SerializeField] TextMeshProUGUI textoStats;

    bool saludoAlEntrar = false;
    bool preguntoSiOcupado = false;
    bool preguntoDudas = false;
    int levantarManoVeces = 0;



    public override void StartMission()
    {
       seleccionSaludoController.OnInfoUpDate += ChangeInstructions;
        seleccionSaludoController.OnEndSaludo += EndSeleccionSaludo;
        
        seleccionSillaController.OnEndSilla += EndSeleccionSilla;
        seleccionPreguntaController.OnInfoUpDate += ChangeInstructions;
        seleccionPreguntaController.OnEndPregunta += EndSeleccionPregunta;


        claseExplicacionController.OnEndExplicacion += EndClaseExplicacion;
        claseExplicacionController.OnInfoUpDate += ChangeInstructions;
        claseEjercicioController.OnEndEjercicio += EndClaseEjercicio;
        claseEjercicioController.OnInfoUpDate += ChangeInstructions;

        Restart();

        textInstrucciones.text = "Elige entre saludar a la clase o ir directamente a tu asiento";

        base.StartMission();
    }

    void Restart()
    {
        seleccionPreguntaController.Restart();
        seleccionSaludoController.Restart();
        seleccionSillaController.Restart();
        claseEjercicioController.Restart();
        claseExplicacionController.Restart();
        buttonEmpezar.onClick.RemoveAllListeners();
    }

    //PRIMERA PARTE MISION
    protected override void AfterInstructions()
    {
        buttonInfo.gameObject.SetActive(true);
        seleccionSaludoController.ActivarSeleccionSaludo();
    }

    void EndSeleccionSaludo(bool s)
    {
        saludoAlEntrar = s;
        buttonInfo.enabled = false;
        seleccionSaludoController.gameObject.SetActive(false);
        Invoke("StartSeleccionSilla",2.5f);
       
    }

    void StartSeleccionSilla()
    {
        Debug.Log("Empieza seleccion de silla");
        ChangeInstructions("Elige dónde te sentarás durante la clase");
        buttonInfo.enabled = true;
        seleccionSillaController.ActivarSeleccionSilla();
    }
    
    void EndSeleccionSilla(Silla s)
    {
        Debug.Log("Terminar seleccion de silla");
        buttonInfo.enabled = true;
        ChangeInstructions("Elige entre preguntar por la disponibilidad del asiento o directamente sentarse");
        seleccionSillaController.gameObject.SetActive(false);
        actualSilla = s;
        seleccionPreguntaController.ActivarSeleccionPregunta(actualSilla);
    }

    void EndSeleccionPregunta(bool ocupado)
    {
        preguntoSiOcupado = ocupado;
        seleccionPreguntaController.gameObject.SetActive(false);
        Invoke("PrepararClase", 3.0f);
    }

    //SEGUNDA PARTE MISION

    void PrepararClase()
    {
        textoSeleccionGO.SetActive(false);
        buttonInfo.gameObject.SetActive(false);

        //Cambio de posicion del player
        GameManager.Instance.SetPlayerTransform(actualSilla.sittingPosition, new Quaternion(0, 0, 0, 0));


        //Cambiar todas las instrucciones
        titulo.text = "Atiende y participa en clase";
        objetivo.text = "Objetivo: atiende y respónde a las preguntas respetando el turno de palabra";
        indicaciones.text = "- Presta atención al profesor\n- Levanta la mano cuando te sepas la respuesta\n- Espera que te den el turno de palabra para hablar\n- Alza la voz al responder\n-No tengas miedo de equivocarte";
        instructions.SetActive(true);

        //Nuevo Listener para el Botton empezar
        buttonEmpezar.onClick.RemoveAllListeners();
        buttonEmpezar.onClick.AddListener(ButtonEmpezarClase);

      
       
    }

    void ButtonEmpezarClase()
    {
        instructions.SetActive(false);
        ActivarClase();
        buttonInfo.gameObject.SetActive(true);
        pizarra.SetActive(true);
        ChangeInstructions("Atiende a la explicación del profesor para poder realizar los ejercicios");
    }

    void ActivarClase()
    {
        //Mostrar la pizarra y el profesor
        pizarra.gameObject.SetActive(true);
        profesor.gameObject.SetActive(true);
        claseExplicacionController.ActivarClaseExplicacion();
    }

    public void EndClaseExplicacion(bool dudas)
    {
        preguntoDudas = dudas;
        claseEjercicioController.ActivarClaseEjercicio();
    }

  
    public void EndClaseEjercicio(int veces)
    {
        levantarManoVeces = veces;
        CreateStats();
        buttonInfo.gameObject.SetActive(false);
        Invoke("ShowStats", 3.0f);
    }

    void CreateStats()
    {
        if (saludoAlEntrar)
        {
            textoStats.text = "* <color=#77DD77>Saludaste a tus compañeros de clase al entrar</color>\n";
        }
        else
        {
            textoStats.text = "* <color=#FF6961>Fuiste directo a tu asiento al entrar</color>\n";

        }

        if (preguntoSiOcupado)
        {
            textoStats.text += "* <color=#77DD77>Preguntaste si el asiento estaba ocupado y conseguiste que quitaran los objetos que lo ocupaban</color>\n";
        }
        else
        {
            textoStats.text += "* <color=#FF6961>No preguntaste si el asiento estaba ocupado y te sentaste en un sitio con objetos ajenos</color>\n";

        }

        if (preguntoDudas)
        {
            textoStats.text += "* <color=#77DD77>Levantaste la mano para indicar al profesor que te quedaron dudas</color>\n";
        }
        else
        {
            textoStats.text += "* <color=#FF6961>No levantaste la mano cuando el profesor preguntó si alguien tenía dudas</color> \n";

        }


        textoStats.text += "* Levantaste la mano para responder un total de " + levantarManoVeces + " preguntas";
    }

}
