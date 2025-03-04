using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Maneja las interacciones en la Misi�n Secretaria
/// </summary>
public class MissionSecretaria : OpenAIFather
{
    //START
    private void Start()
    {
        openAI.transform.SetPositionAndRotation(npcPrefab.GetComponent<NpcAIPosition>().OpenAIPosition.position, 
            npcPrefab.GetComponent<NpcAIPosition>().OpenAIPosition.rotation); //Colocar el gameObject de OpenAI para que coincida con el NPC de Secretaria

    }

    public override void StartMission() 
    {
        missionName = "Misi�n Secretar�a";

        stats.InitializeMissionDictionary();//Inicializar las stats    
        
        json.SetManagerOpenAI(openAI.GetComponent<ManagerOpenAI>());//Indicar el ManagerOpenAI en el json

        ReestartStats();//Reiniciar el UI de las Stats
        textInstrucciones.text = "Inicia la conversaci�n con un saludo:\n * Hola / Buenos d�as";
        
        openAI.GetComponent<ManagerOpenAI>().ReestartManagerOpenAI();//Reiniciar la IA
        //Llamar a la clase base
        base.StartMission();
    }

    //OVERRIDE METHODS START

    protected override void InitializeUserKeyInteractions()
    {
        keyUserInteractions = new List<KeyInteraction>
        {
            new KeyInteraction("Saludo", new List<string> { "hola", "buenos d�as" }, "Pregunta por el carn� de estudiante: " +
            "\n* �Me puedes decir c�mo conseguir el carn� de estudiante porfavor?" +
            "\n * Recuerda pedir las cosas por favor y dar las gracias"),

            new KeyInteraction("Despedida", new List<string> { "hasta luego", "adi�s" },null),
            new KeyInteraction("Por favor", new List<string> { "por favor"},null),
            new KeyInteraction("Gracias", new List<string> { "gracias"},null),

            new KeyInteraction("Pedir el carnet", new List<string> { "carn�", "carnet" },
            "Responde las preguntas que te haga la secretaria de forma clara"),
            new KeyInteraction("Consulta Ventajas", new List<string> { "ventajas" },
            "Pregunta por algo m�s espec�fico:" +
            "\n *�A qu� ofertas puedo acceder con el carn�?" +
            "\n*�C�mo saco libros de la biblioteca?")
        };
    }

    protected override void InitializeOpenAiKeyInteractions()
    {
        keyOpenAiInteractions = new List<KeyInteraction>
        {
             new KeyInteraction("Pedir el carnet", new List<string> { "carn�", "carnet" }, "Pregunta por las ventajas del carn� de estudiante: " +
            "\n *�Qu� ventajas tiene el carn�?"),

            new KeyInteraction("Algo m�s", new List<string> { "algo m�s" }, "Pregunta por las ventajas del carn� de estudiante: " +
            "\n *�Qu� ventajas tiene el carn�?"),

  
            new KeyInteraction("Consulta Ventajas", new List<string> { "ventajas" },
            "Pregunta por algo m�s espec�fico:\n *�A qu� ofertas puedo acceder con el carn�?\n*�C�mo saco libros de la biblioteca?"),

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

                ChangeInstructions(interaction.Instruction);
            }      
        }   
    }

  

    protected override void ButtonListo()
    {
        openAI.SetActive(true);
        base.ButtonListo();
    }
    protected override void ButtonInfo()
    {
        openAI.SetActive(false);
        base.ButtonInfo();
    }
    protected override void AfterInstructions()
    {
        instruccionsInGame.SetActive(true);
        buttonInfo.enabled = false;
        buttonInfo.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttonListo.gameObject);
        
    }


    //OVERRIDE METHODS END

}
