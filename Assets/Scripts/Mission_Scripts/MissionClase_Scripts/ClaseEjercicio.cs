using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///Maneja la parte de la Mision Clase en la que el usuario debe participar en clase.
/// </summary>
public class ClaseEjercicio : MonoBehaviour
{
    [SerializeField] Button levantarManoResponder;
    bool manoLevantada = false;
    private Coroutine countdownCoroutine;

    [SerializeField] GameObject levantarManoDudasGO;
    [SerializeField] Button noLevantarManoDudas;
    [SerializeField] Button levantarManoDudas;
    [SerializeField] ProfesorManager profesor;

    [SerializeField] AudioClip levantadLaMano;
    [SerializeField] AudioClip audioCorrecto;
    [SerializeField]List<AudioClip> audioIncorrecto;
    [SerializeField] List<AudioClip> audioExplicacion;
    [SerializeField] AudioClip audioSiguiente;

    [SerializeField] TextMeshProUGUI textoSeleccion;
    [SerializeField] GameObject textSeleccionGO;

    [SerializeField] List<GameObject> npcs;
    [SerializeField] List<AudioClip> respuestasNpcs;

    [SerializeField] ListenerOpenAI openAIController;

    public event Action<int>OnEndEjercicio;
    [SerializeField] TextMeshProUGUI pizarra;

    int levantarManoVeces = 0;

    public event Action<string> OnInfoUpDate;

    int numEjercicio=0;
    List<string> preguntas = new List<string> { "18, 14, 10, 6, _", "2, 4, 8, 16, _", "81, 27, 9, 3, _", "2, 3, 5, 8, 13, 21, _" };
    List<string> respuestasNumero = new List<string> { "2", "32", "1", "34" };
    List<string> respuestasPizarra = new List<string> {  "18, 14, 10, 6, 2", "2, 4, 8, 16, 32", "81, 27, 9, 3, 1", "2, 3, 5, 8, 13, 21, 34" };
    List<string> respuestasLetra = new List<string> { "dos", "treinta y dos", "uno", "treinta y cuatro" };

    private void Start()
    {
        
        levantarManoResponder.onClick.AddListener(LevantarManoResponder);
        levantarManoDudas.onClick.AddListener(LevantarManoDudas);
        noLevantarManoDudas.onClick.AddListener(NoLevantarManoDudas);

        //Cuando el usuario responda comprobamos si es correto
        openAIController.OnUserResponse += ComprobarRespuesta;
    }

    public void ActivarClaseEjercicio() {
        this.gameObject.SetActive(true);
        Debug.Log("CLASE EJERCICIO ACTIVADA");
        PresentarEjercicio();
        StartCountdown();
    }

    public void Restart()
    {
        profesor.StopAudio();
        levantarManoDudasGO.SetActive(false);
        levantarManoResponder.gameObject.SetActive(false);
        pizarra.text =" ";
        levantarManoVeces = 0;
    }

    private void LevantarManoNpcs()
    {
        foreach (GameObject npc in npcs)
        {
            npc.GetComponent<Animator>().SetBool("BajarMano", false);
            npc.GetComponent<Animator>().SetBool("LevantarMano", true);

        }

    }

    private void LevantarManoResponder()
    {
        levantarManoVeces++;
        OnButtonPressed();
        OnInfoUpDate?.Invoke("Espera a tener el turno de palabra antes de responder");
        textoSeleccion.text ="Tienes la mano levantada, espera a que te den el turno de palabra";
        levantarManoResponder.gameObject.SetActive(false);
        textoSeleccion.color =new Color(0.9f, 0.9f, 0.9f, 1f);
        Invoke("DarTurnoDePalabra", 5.0f);
       
    }

    void StartCountdown()
    {
        if (countdownCoroutine != null)
        {
            Debug.Log("Deteniendo la corrutina anterior...");
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = StartCoroutine(CheckButtonPress());
        Debug.Log("Nueva cuenta atrás iniciada: " + countdownCoroutine);
    }

    void OnButtonPressed()
    {
        manoLevantada = true;
        Debug.Log("Sí lo pulsó");

        if (countdownCoroutine != null)
        {
            Debug.Log("Deteniendo la cuenta atrás: " + countdownCoroutine);
            StopCoroutine(countdownCoroutine);
            
            countdownCoroutine = null;
        }
    }
    IEnumerator CheckButtonPress()
    {
        Debug.Log("Esperando 15 segundos...");
        yield return new WaitForSeconds(15);

        if (!manoLevantada)
        {
            
            Debug.Log("No lo pulsó");
            foreach (GameObject npc in npcs)
            {
                npc.GetComponent<Animator>().SetBool("BajarMano", true);
                npc.GetComponent<Animator>().SetBool("LevantarMano", false);
            }
            RespondeNpc();
            levantarManoResponder.gameObject.SetActive(false);
        }

        Debug.Log("Corrutina terminada, reseteando referencia.");
        countdownCoroutine = null;
    }
    void DarTurnoDePalabra()
    {
        foreach (GameObject npc in npcs)
        {
            npc.GetComponent<Animator>().SetBool("BajarMano", true);
            npc.GetComponent<Animator>().SetBool("LevantarMano", false);
        }

        switch (numEjercicio)
        {
            case 0:
                RespondeNpc();
                break;
            case 1:
                RespondeUsuario();
                break;
            case 2:
                RespondeNpc();
                break;
            case 3:
                RespondeUsuario();
                break;
        }
    }
    void RespondeUsuario()
    {
        OnInfoUpDate?.Invoke("Tienes el turno de palabra, pulsa 'Responder' y di la respuesta del ejercicio");
        textoSeleccion.text = "Tienes el turno de palabra";
        textoSeleccion.color = Color.green;

        openAIController.gameObject.SetActive(true);


    }

    void RespondeNpc()
    {
        OnInfoUpDate?.Invoke("Otro compañero tiene el turno de palabra, escucha su respuesta");
        textoSeleccion.text = "Otro compañero tiene el turno de palabra";
        textoSeleccion.color = new Color(1f, 0.6f, 0.6f, 1f);


        Invoke("RespuestaAlumno", 3.0f);

    }
    void RespuestaAlumno()
    {
      
        npcs[numEjercicio].GetComponent<AudioSource>().clip = respuestasNpcs[numEjercicio];
        npcs[numEjercicio].GetComponent<AudioSource>().Play();

        Invoke("RespuestaCorrectaProfesor", 2.0f);
    }

    void RespuestaInCorrectaProfesor()
    {

        profesor.PlayAudio(audioIncorrecto[numEjercicio]);
        PreguntarPorDudas();
    }
    void RespuestaCorrectaProfesor()
    {

        profesor.PlayAudio(audioCorrecto);
        PreguntarPorDudas();
    }

    void PreguntarPorDudas()
    {
        OnInfoUpDate?.Invoke("Levanta la mano para que el profesor lo vuelva a explicar si es necesario");
        pizarra.text = respuestasPizarra[numEjercicio];
        profesor.GetComponent<Animator>().SetBool("Reiniciar", false);
        profesor.GetComponent<Animator>().SetBool("EsCorrecto", true);
        textoSeleccion.text = "Levanta la mano si no lo has entendido";
        textoSeleccion.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        levantarManoDudasGO.SetActive(true);
    }

    void LevantarManoDudas()
    {
        OnInfoUpDate?.Invoke("Escucha la explicación del profesor");
        profesor.PlayAudio(audioExplicacion[numEjercicio]);
        levantarManoDudasGO.SetActive(false);
        textSeleccionGO.SetActive(false);
        var margen = audioExplicacion[numEjercicio].length + 1.0f;
        Invoke("SiguienteEjercicio", margen);
        
    }

    void NoLevantarManoDudas()
    {
        OnInfoUpDate?.Invoke("Levanta la mano para tener el turno de palabra");
        textSeleccionGO.SetActive(false);
        SiguienteEjercicio();
        
       
    }

    void SiguienteEjercicio()
    {
        manoLevantada = false;
        numEjercicio++;
        levantarManoDudasGO.SetActive(false);
        if (numEjercicio < preguntas.Count)
        {
            profesor.PlayAudio(audioSiguiente);
                      
            var margen = audioSiguiente.length + 1.0f;
            Invoke("PresentarEjercicio", margen);
        }
        else
        {
            OnEndEjercicio?.Invoke(levantarManoVeces);
        }       
    }

   

    public void PresentarEjercicio()
    {
        StartCountdown();
        profesor.PlaySinglePaso(new PasoClase(levantadLaMano, preguntas[numEjercicio])); 

        textSeleccionGO.SetActive(true);
        textoSeleccion.text = "Levanta la mano para contestar";
        levantarManoResponder.gameObject.SetActive(true);
        LevantarManoNpcs();
        
    }
    void ComprobarRespuesta(string s)
    {
        openAIController.gameObject.SetActive(false);
        var rL = respuestasLetra[numEjercicio];
        var rN = respuestasNumero[numEjercicio];
      
          if(s.IndexOf(rL, System.StringComparison.OrdinalIgnoreCase) != -1 || s.IndexOf(rN, System.StringComparison.OrdinalIgnoreCase) != -1)
          {
           
            RespuestaCorrectaProfesor();
          }
          else
          {
            RespuestaInCorrectaProfesor();
          }     
    }
}
