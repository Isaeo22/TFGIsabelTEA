using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase padre de las misiones que usan OpenAI (Secretaria, Cafeteria y Amigos).
/// Maneja las respuestas del usuario y de la IA.
/// </summary>
public abstract class OpenAIFather : MissionFather
{

    protected List<KeyInteraction> keyUserInteractions;//Interacciones del ususario
    protected List<KeyInteraction> keyOpenAiInteractions; //Interacciones con OpenAI

    [Header("OpenAI GameObject")]
    [SerializeField] protected GameObject openAI; //El gameObject del OpenAI

    [Header("NPC Prefab")]
    [SerializeField] protected GameObject npcPrefab;

    //Variables ready
    private string userMessage;
    private string chatGPTMessage;
    Color colorStats;
    public override void StartMission()
    {
        //Inicializamos las interacciones que queramos
        //comprobar si el usuario las ha hecho (Saludar, Despedirse, Dar las gracias)
        InitializeUserKeyInteractions();
        InitializeOpenAiKeyInteractions(); 

        base.StartMission();
    }

    //OVERRIDE METHODS START
    protected override void ButtonEmpezar()
    {
        base.ButtonEmpezar();
    }
    //OVERRIDE METHODS END

    //VIRTUAL METHODS START
    /// <summary>
    /// Inicializa las interacciones clave del usuario
    /// </summary>
    protected virtual void InitializeUserKeyInteractions() {}
    /// <summary>
    /// Inicializa las interacciones clave de la IA
    /// </summary>
    protected virtual void InitializeOpenAiKeyInteractions() {}

    /// <summary>
    /// Analiza las respuestas del usuario para saber si ha cumplido con el objetivo y pasa al siguiente paso de la interaccion.
    /// </summary>
    protected virtual void AnalyzeUserMessage() {}
    /// <summary>
    /// Analiza las respuestas de la IA para saber si el usuario ha cumplido con el objetivo y pasa al siguiente paso de la interaccion.
    /// </summary>
    protected virtual void AnalyzeChatGPTMessage() {}
    //VIRTUAL METHODS END

    //PROTECTED METHODS START
    public void SetAnimationsVariables(bool b) {

        npcPrefab.GetComponent<Animator>().SetBool("IsTalking", b);

        int rand = UnityEngine.Random.Range(0, 2);

        npcPrefab.GetComponent<Animator>().SetBool("WithGesture", rand == 0);


        npcPrefab.GetComponent<Animator>().SetBool("IsWaitingForResponse", !b);
    }


    protected void CloseChat()//Si el usuario se despide, se cierra el chat
    {
        openAI.SetActive(false);
        buttonInfo.gameObject.SetActive(false);
        cronometer.StopStopwatch();
       
        stats.SetEyetrackingPercentage(eyetrackerObject.GetEyetrackingPercentage());

        json.SaveToJson();

        Invoke("ShowStats", 2.0f);
    }

    protected override void ButtonInfo()
    {  
        openAI.SetActive(false);
        base.ButtonInfo();
    }

    protected override void ButtonListo()
    {
        openAI.SetActive(true);
        base.ButtonListo();
    }

    //PROTECTED METHODS END

    /// <summary>
    /// Metodo que analiza la respuesta del usuario
    /// </summary>
    public void SetUserMessage(string m)
    {
        userMessage = m;

        AnalyzeUserMessage();
        AddNumWordsMessage(userMessage);
        AddNumMessages();
    }

    public string GetUserMessage()
    {
        return userMessage;
    }

    /// <summary>
    /// Metodo que analiza la respuesta de la IA
    /// </summary>
    public void SetChatGPTMessage(string m)
    {
        chatGPTMessage = m;
        AnalyzeChatGPTMessage();
    }
    public string GetChatGPTMessage()
    {
        return chatGPTMessage;
    }

    protected void ReestartStats()
    {
        statsGO.SetActive(false);
        if (ColorUtility.TryParseHtmlString("#F37141", out colorStats))
        {
            for (int i = 0; i < stats.statList.Count; i++)
            {
                stats.statList[i].color = colorStats;
                stats.statList[i].sprite = spriteUnchecked;
            }
        }
    }
}
