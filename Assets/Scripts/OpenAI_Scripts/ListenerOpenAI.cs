using OpenAI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Samples.Whisper;
using System;

public class ListenerOpenAI : MonoBehaviour
{
    [SerializeField] public Button buttonResponder;
    [SerializeField] public GameObject sliderHablaAhora;
    [SerializeField] public GameObject buttonsAceptarRehacer;
    [SerializeField] Button buttonAceptar;
    [SerializeField] Button buttonRehacer;
    [SerializeField] GameObject textBox;

    public event Action<string> OnUserResponse;
    public event Action OnRecording;

    //[Header("Pruebas")]
    //[SerializeField] public TMP_Dropdown dropDownMics;

    private float time;
    private readonly int duration = 5;
    public bool isRecording;
    private AudioClip clip;

    [SerializeField] private Slider timeBar;
    [SerializeField] private TMP_Text message;

    public string userMessage;
    public bool isWaitingResponse;


    private readonly string fileName = "output.wav";

    private OpenAIApi openai = new OpenAIApi("sk-WZbk79H5Sl_WLKEOiYOiV4JckcwHdYtadZrSVhAuzUT3BlbkFJacrLkq3qPpCiGLmuZ-y2cqduFsWlSbsOy5fybaheYA", "org-dPg0qtvmdXdTHQ55u1DhoiyH");

   
    public int messageRepetition;
    int micIndex;
    string micName;

    List<string> hallucinations = new List<string>()
    {
        "Un besito y nos vemos en el pr�ximo v�deo",
        "Un besito y nos vemos en el proximo video",
        "�Gracias por ver el video!",
        "Subt�tulos realizados por la comunidad de amara.org",
        "Y bueno, esto es todo por el d�a de hoy, nos vemos en el pr�ximo v�deo, �hasta la pr�xima!",
        "Subt�tulos por la comunidad de Amara.org",
        "Gracias por ver el v�deo, suscribanse a mi canal y activen la campanita para ver m�s v�deos.",
        "Gracias por ver el video",
        "Un saludo y nos vemos en el siguiente video.",
        "Y nos vemos en el pr�ximo v�deo.",
        "Un saludo y nos vemos en el siguiente video, �adi�s!",
        "Gracias por ver el video.",
        "Un saludo y hasta la pr�xima.",
        "Y nos vemos en el pr�ximo video, �hasta la pr�xima!",
        "�Gracias por ver!",
        "Un saludo y nos vemos en el pr�ximo v�deo, �chao!",
        "Gracias por ver el v�deo, �nos vemos en el pr�ximo!",
          "�Hasta la pr�xima!"
    };
    // Start is called before the first frame update
    public void Start()
    {
        buttonResponder.onClick.AddListener(StartListener);
        buttonRehacer.onClick.AddListener(StartListener);
        buttonRehacer.onClick.AddListener(CountMessageRepetition);
        buttonAceptar.onClick.AddListener(RestartListener);

     
        messageRepetition = 0;
        
    }

    private void StartListener()
    {        
        buttonResponder.gameObject.SetActive(false);
        sliderHablaAhora.SetActive(true);
        message.text = " ";
        StartRecording();
    }

    private void StartRecording()
    {
        buttonsAceptarRehacer.SetActive(false);
        OnRecording?.Invoke();
        isRecording = true;

        var micName = PlayerPrefs.GetString("userMic");  // Valor por defecto si no se encuentra el nombre


        #if !UNITY_WEBGL
        clip = Microphone.Start(micName, false, duration, 44100);
        #endif
    }

    private async void EndRecording()
    {
        textBox.SetActive(true);
        sliderHablaAhora.SetActive(false);
        message.text = "Escuchando...";
       
        #if !UNITY_WEBGL
        Microphone.End(null);
        #endif

        byte[] data = SaveWav.Save(fileName, clip);

        var req = new CreateAudioTranscriptionsRequest
        {
            FileData = new FileData() { Data = data, Name = "audio.wav" },
            // File = Application.persistentDataPath + "/" + fileName,
            Model = "whisper-1",
            Language = "es"
        };
        var res = await openai.CreateAudioTranscription(req);

        timeBar.value = 0;

        FilterMessage(res.Text);
        

        SetMessage(message.text);
        buttonsAceptarRehacer.SetActive(true);

        buttonsAceptarRehacer.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttonAceptar.gameObject);


    }

    void FilterMessage(string m)
    {
        foreach (string h in hallucinations)
        {
            if (string.Equals(m, h, StringComparison.OrdinalIgnoreCase))
            {
                message.text = "(...)";
                return;
            }
            else
            {
                message.text = m;
            }


        }
    }

    void SetMessage(string m)
    {
       
        userMessage = m;
    }

    private void RestartListener()
    {
        buttonResponder.gameObject.SetActive(true);
        buttonsAceptarRehacer.SetActive(false);
        message.text = " ";

        OnUserResponse?.Invoke(userMessage);
        isWaitingResponse = true;
        
    }

    private void Update()
    {
        if (isRecording)
        {
            time += Time.deltaTime;
            timeBar.value = time / duration;

            if (time >= duration)
            {
                time = 0;
                isRecording = false;
                EndRecording();
            }
        }
    }


    private void CountMessageRepetition()
    {
        messageRepetition++;
    }


  

    // Detener la grabaci�n si el jugador sale de la misi�n
    public void StopRecording()
    {
        if (isRecording)
        {
            
            isRecording = false;
            Debug.Log("Grabaci�n detenida debido a que el jugador ha salido.");
        }
    }
}
