using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class ManagerOpenAI : MonoBehaviour
{
    [SerializeField]public ListenerOpenAI listener;
    [SerializeField]TalkerOpenAI talker;
    [SerializeField] OpenAIFather mission;

    [SerializeField] string firstMessage;
    [SerializeField] string promt;
    [SerializeField] string rol;
    [SerializeField] public TextMeshProUGUI rolText;
    [SerializeField] public TextMeshProUGUI messageText;


    [SerializeField] Button buttonAcabarConvo;
    [SerializeField] GameObject panelSiNoAcabarConvo;
    [SerializeField] GameObject canvasAcabarConvo;


    [SerializeField] Button buttonSi;
    [SerializeField] Button buttonNo;

    [HideInInspector]
    public string messageListener;
    [HideInInspector]
    public string messageTalker;

    private float totalResponseTime;  // Tiempo total de respuestas
    private int totalResponsesCount;  // Número total de respuestas

   [SerializeField] Crono cronoTotalResponseTime;

    private void Awake()
    {
        buttonAcabarConvo.onClick.AddListener(ButtonAcabarConvo);
   
    }

    private void Start()
    {
        listener.OnUserResponse += SetUserResponse;
        listener.OnRecording += SetRecording;
        talker.OnNpcResponse += SetNpcResponse;
    }

    public void SetUserResponse(string userMessage)
    {          
            mission.SetUserMessage(userMessage);

        

            totalResponsesCount++;  // Incrementar el número de respuestas

            talker.message.text = "  ";

            mission.SetAnimationsVariables(true);

            talker.AskChatGPT(userMessage);

            talker.gameObject.SetActive(true);
            cronoTotalResponseTime.StopStopwatch();

    }
    public void SetNpcResponse(string npcMessage)
    {


        // questionStartTime = Time.time;  // Registrar el tiempo al iniciar la pregunta

        //Debug.Log("Tiempo de respuesta de OpenAI: " + questionStartTime + " segundos");

        cronoTotalResponseTime.StartStopwatch();
       
       mission.StopEyeTrackerTime();
        
           
        mission.SetChatGPTMessage(npcMessage);
        

    }


    void SetRecording()
    {
       
        mission.StartEyeTrackerTime();
        
            
        
            

        mission.SetAnimationsVariables(false);
        talker.gameObject.SetActive(false);
    }

    void ButtonAcabarConvo()
    {
        panelSiNoAcabarConvo.SetActive(true);     
        EventSystem.current.SetSelectedGameObject(buttonNo.gameObject);

    }



    public void ReestartManagerOpenAI()
    {
        talker.gameObject.SetActive(true);
        listener.buttonResponder.gameObject.SetActive(true);
        listener.sliderHablaAhora.SetActive(false);
        listener.buttonsAceptarRehacer.SetActive(false);



        cronoTotalResponseTime.StartStopwatch();

        talker.firstMessage = firstMessage;
        talker.prompt = promt;
        talker.rol = rol;

        talker.StartTalker();
        listener.StopRecording();
        talker.message.text =firstMessage;
        this.gameObject.SetActive(false);
    }

  

   public int GetNumMessages()
    {
        return talker.messages.Count;
    }

    public int GetMessageRepetition()
    {
        return listener.messageRepetition;
    }

    // Método para obtener el tiempo medio de respuesta
    public float GetAverageResponseTime()
    {
        if (totalResponsesCount == 0)
            return 0f;  // Evitar división por cero si no hay respuestas

        totalResponseTime = cronoTotalResponseTime.GetElapsedTime();
        return totalResponseTime / totalResponsesCount;
    }

}
