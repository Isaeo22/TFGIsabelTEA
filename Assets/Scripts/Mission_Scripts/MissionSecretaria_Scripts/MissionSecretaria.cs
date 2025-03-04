using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Maneja las interacciones en la Misión Secretaria
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
        missionName = "Misión Secretaría";

        stats.InitializeMissionDictionary();//Inicializar las stats    
        
        json.SetManagerOpenAI(openAI.GetComponent<ManagerOpenAI>());//Indicar el ManagerOpenAI en el json

        ReestartStats();//Reiniciar el UI de las Stats
        textInstrucciones.text = "Inicia la conversación con un saludo:\n * Hola / Buenos días";
        
        openAI.GetComponent<ManagerOpenAI>().ReestartManagerOpenAI();//Reiniciar la IA
        //Llamar a la clase base
        base.StartMission();
    }

    //OVERRIDE METHODS START

    protected override void InitializeUserKeyInteractions()
    {
        keyUserInteractions = new List<KeyInteraction>
        {
            new KeyInteraction("Saludo", new List<string> { "hola", "buenos días" }, "Pregunta por el carné de estudiante: " +
            "\n* ¿Me puedes decir cómo conseguir el carné de estudiante porfavor?" +
            "\n * Recuerda pedir las cosas por favor y dar las gracias"),

            new KeyInteraction("Despedida", new List<string> { "hasta luego", "adiós" },null),
            new KeyInteraction("Por favor", new List<string> { "por favor"},null),
            new KeyInteraction("Gracias", new List<string> { "gracias"},null),

            new KeyInteraction("Pedir el carnet", new List<string> { "carné", "carnet" },
            "Responde las preguntas que te haga la secretaria de forma clara"),
            new KeyInteraction("Consulta Ventajas", new List<string> { "ventajas" },
            "Pregunta por algo más específico:" +
            "\n *¿A qué ofertas puedo acceder con el carné?" +
            "\n*¿Cómo saco libros de la biblioteca?")
        };
    }

    protected override void InitializeOpenAiKeyInteractions()
    {
        keyOpenAiInteractions = new List<KeyInteraction>
        {
             new KeyInteraction("Pedir el carnet", new List<string> { "carné", "carnet" }, "Pregunta por las ventajas del carné de estudiante: " +
            "\n *¿Qué ventajas tiene el carné?"),

            new KeyInteraction("Algo más", new List<string> { "algo más" }, "Pregunta por las ventajas del carné de estudiante: " +
            "\n *¿Qué ventajas tiene el carné?"),

  
            new KeyInteraction("Consulta Ventajas", new List<string> { "ventajas" },
            "Pregunta por algo más específico:\n *¿A qué ofertas puedo acceder con el carné?\n*¿Cómo saco libros de la biblioteca?"),

        };
    }

  
    protected override void AnalyzeUserMessage()
    {
        string userMessage = GetUserMessage();

        foreach (var interaction in keyUserInteractions)
        {
            // Verificar si la misión se completa con alguna de las palabras detectadas.
            interaction.CheckCompletion(userMessage);

            // Si la misión se ha completado, mostrar la instrucción correspondiente.
            if (interaction.IsCompleted && !interaction.HasBeenNotified)
            {
                interaction.HasBeenNotified = true;
                stats.UpdateStatImage(interaction.InteractionName); // Actualizar la imagen de la misión completada.
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
            // Verificar si la misión se completa con alguna de las palabras detectadas.
            interaction.CheckCompletion(chatGPTMessage);

            // Si la misión se ha completado, mostrar la instrucción correspondiente.
            if (interaction.IsCompleted && !interaction.HasBeenNotified)
            {
                interaction.HasBeenNotified = true;
                stats.UpdateStatImage(interaction.InteractionName); // Actualizar la imagen de la misión completada.

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
