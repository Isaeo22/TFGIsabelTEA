using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
///Esta clase maneja las interacciones del usuario con varios estudiantes de la universidad.
/// </summary>
public class MisionAmigos : OpenAIFather
{
    //Despues de cada interaccion se pregunta si continuar o salir de la mision
    [SerializeField] Button buttonContinuar;
    [SerializeField] Button buttonAcabarMisionDef;

    //El npc con el que se est� hablando
    NpcManager actualNpc;

    int numFriends = 0;
    int totalNpcs=6;

    [Header("Configuraci�n Selecci�n")]
    [SerializeField] SeleccionDePersonaje seleccionPersonajeController;
    bool nameCompleted = false;
    
    public override void StartMission()
    {

        missionName = "Misi�n Amigos";

        InitializeUserKeyInteractions();

        stats.InitializeMissionDictionary();

        buttonContinuar.onClick.AddListener(ButtonContinuar);

        buttonAcabarMisionDef.onClick.AddListener(ButtonAcabarMision);
        


        seleccionPersonajeController.OnNpcSelected += StartTalk;  

        seleccionPersonajeController.RestartMision();
        numFriends = 0;
        base.StartMission();
    }

    //Se llama despues de elegir a alguien para hablar
    private void StartTalk(NpcManager selectedNpc)
    {
        // Asignar too lo del actual NPC
        actualNpc = selectedNpc;
        npcPrefab=selectedNpc.gameObject;
        openAI = actualNpc.openAI;
        openAI.GetComponent<ManagerOpenAI>().ReestartManagerOpenAI();      
        openAI.SetActive(true);
        openAI.transform.SetPositionAndRotation(npcPrefab.GetComponent<NpcAIPosition>().GetOpenAIPosition(), actualNpc.GetComponent<NpcAIPosition>().GetOpenAIRotation());
        npcPrefab.GetComponent<Animator>().SetBool("OnChat", true);

        //Reiniciar todo para la interaccion actual
        json.SetManagerOpenAI(openAI.GetComponent<ManagerOpenAI>());
        ReestartEyetracker();
        SetEyetrackerPos();  
        ReestartJson();
        cronometer.StartStopwatch();
        buttonInfo.gameObject.SetActive(true);

        //Inicializacion de las interacciones clave
        InitializeUserKeyInteractions();
        keyOpenAiInteractions = npcPrefab.GetComponent<NpcStats>().GetKeyInteractions();
    }

    private void SetEyetrackerPos()
    {
        Transform targetHead = actualNpc.GetComponent<NpcManager>().head.transform;
        if (targetHead != null)
        {
            // Establecer el EyeTrackerObject como hijo de la cabeza del NPC
            eyetrackerObject.gameObject.transform.SetParent(targetHead);

            // Reiniciar la posici�n local para alinear con la cabeza del NPC

            eyetrackerObject.gameObject.transform.localPosition = Vector3.zero;
            eyetrackerObject.gameObject.transform.localRotation = Quaternion.identity;

        }
        eyetrackerObject.gameObject.SetActive(true);


    }

    protected override void InitializeUserKeyInteractions()
    {
        keyUserInteractions = new List<KeyInteraction>
        {
            new KeyInteraction("Saludo", new List<string> { "hola", "buenos d�as" }, "Pregunta por su nombre:\n *�C�mo te llamas?"),
            new KeyInteraction("Nombre", new List<string> { "nombre" }, null),
            new KeyInteraction("Estudios", new List<string> { "estudias", "carrera", "estudiando" }, null),
            new KeyInteraction("Musica", new List<string> { "m�sica", "musica" }, null),
            new KeyInteraction("Deporte", new List<string> { "deporte" }, null),
            new KeyInteraction("Pasatiempo", new List<string> { "tiempo", "pel�cula", "pelicula", "serie" }, null),
            new KeyInteraction("Mascota", new List<string> { "mascota", "animal" }, null),
            new KeyInteraction("Despedida", new List<string> { "hasta luego", "adi�s" }, null)
        };
    }

    protected override void AnalyzeUserMessage()
    {
        string userMessage = GetUserMessage();
        foreach (var interaction in keyUserInteractions)
        {
            // Verificar si la misi�n se completa con alguna de las palabras detectadas.
            interaction.CheckCompletion(userMessage);

            // Si la misi�n se ha completado, mostrar la instrucci�n correspondiente.
            if (interaction.IsCompleted && !interaction.HasBeenNotified)
            {
                interaction.HasBeenNotified = true;
                stats.UpdateStatImage(interaction.InteractionName); // Actualizar la imagen de la misi�n completada.
                ChangeInstructions(interaction.Instruction);

                if (interaction.InteractionName == "Despedida")
                {
                    Invoke("CloseChat", 3.0f);
                    eyetrackerObject.gameObject.SetActive(false);
                }
            }
        }
    }

    protected override void AnalyzeChatGPTMessage()
    {

        string chatGPTMessage = GetChatGPTMessage();

        foreach (var interaction in keyOpenAiInteractions)
        {
            // Verificar si la misi�n se completa con alguna de las palabras detectadas.
            interaction.CheckCompletion(chatGPTMessage);

            // Si la misi�n se ha completado, mostrar la instrucci�n correspondiente.
            if (interaction.IsCompleted && !interaction.HasBeenNotified)
            {
                interaction.HasBeenNotified = true;
                stats.UpdateStatImage(interaction.InteractionName); // Actualizar la imagen de la misi�n completada.

                if (interaction.InteractionName == "Nombre")
                {
                    nameCompleted = true;
                    string aux = actualNpc.nombre;
                    openAI.GetComponent<ManagerOpenAI>().rolText.text = aux;

                }

                if (nameCompleted)
                {
                    ChangeInstructions(interaction.Instruction);
                }
            }
        }
    }

    
    private void ButtonContinuar()
    {
        numFriends++;

        //Se reinicia todo para iniciar una nueva interaccion
        ReestartStats();
        seleccionPersonajeController.RestartSelection();
        ReestartJson();

        if (numFriends == totalNpcs)//Mision Acabada
        {

            seleccionPersonajeController.EndSelection();
            buttonAcabarMisionDef.gameObject.SetActive(true);

        }

    }

   
    protected override void AfterInstructions()
    {
        // Configura las instrucciones iniciales de selecci�n de personajes
        textInstrucciones.text = "Inicia la conversaci�n con un saludo:\n * Hola / Buenos d�as";

        buttonInfo.enabled = true;

        seleccionPersonajeController.ActivarSeleccion();
        buttonInfo.gameObject.SetActive(false);
    }
}