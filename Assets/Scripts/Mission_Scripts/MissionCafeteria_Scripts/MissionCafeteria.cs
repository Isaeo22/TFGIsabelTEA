using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
/// <summary>
///Esta clase maneja las interacciones del usuario con un camarero
/// </summary>
public class MissionCafeteria : OpenAIFather
{
    [Header("Mission Cafeteria Variables")]
    [SerializeField] GameObject npcsGameObject;//Npcs dentro de la cafetería
    [SerializeField] Button buttonPagar;

    // Lista de palabras relacionadas con disculpas
    List<string> apologyKeywords;

    private void Start()
    {
        openAI.transform.SetPositionAndRotation(npcPrefab.GetComponent<NpcAIPosition>().OpenAIPosition.position,
            npcPrefab.GetComponent<NpcAIPosition>().OpenAIPosition.rotation); //Colocar el gameObject de OpenAI para que coincida con el NPC de Secretaria

    }
    public override void StartMission()
    {
        missionName = "Misión Cafetería";

        stats.InitializeMissionDictionary();

        json.SetManagerOpenAI(openAI.GetComponent<ManagerOpenAI>());

        ReestartStats();
        textInstrucciones.text = "Inicia la conversación con un saludo:\n * Hola / Buenos días";
        openAI.GetComponent<ManagerOpenAI>().ReestartManagerOpenAI();
        buttonPagar.gameObject.SetActive(false);

        //START MISION CAFETERIA
        buttonPagar.onClick.AddListener(ButtonPagar);
        apologyKeywords = new List<string> { "lo siento", "disculpa", "perdona", "perdón" };
        npcsGameObject.SetActive(true);

        //Llamar a la clase base
        base.StartMission();
    }

    protected override void InitializeUserKeyInteractions()
    {
        keyUserInteractions = new List<KeyInteraction>
        {
             new KeyInteraction("Saludo", new List<string> { "hola", "buenos días" }, "Mira el menú, elige algo y pídelo:\n *Un café y un donut por favor.\n No te olvides de pedir por favor y dar las gracias"),
             new KeyInteraction("Despedida", new List<string> { "hasta luego", "adiós" },null),
             new KeyInteraction("Por favor", new List<string> { "por favor" }, null),
             new KeyInteraction("Gracias", new List<string> { "gracias" }, null),
             new KeyInteraction("Pedir del Menu",  new List<string> { "bocadillo", "hamburguesa", "pizza", "bebida", "batido", "café", "donut", "patatas", "helado" }
            , "Al pedir asegúrate de que el camarero escucha bien la orden,si no, corrígele:\n*Perdona, eso no es lo que he pedido"),
             
        };
    }
    protected override void InitializeOpenAiKeyInteractions()
    {
        keyOpenAiInteractions = new List<KeyInteraction>
        {  
            new KeyInteraction("Corregir precio", new List<string> { "euro", "€", "precio" },
                  "Comprueba el precio de lo que has pedido antes de pagar: \n *Perdona, en el menú no pone ese precio"),

            new KeyInteraction("Corregir orden", new List<string> { "bocadillo", "hamburguesa", "pizza", "bebida", "batido", "café", "donut", "patatas", "helado" }, null),
            
        };
    }

    //Override methods BEGINING
 
    protected override void AnalyzeUserMessage()
    {
        string userMessage = GetUserMessage();
        foreach(var interaction in keyUserInteractions)
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

        foreach (KeyInteraction interaction in keyOpenAiInteractions)
        {
            ProcessInteraction(interaction, chatGPTMessage);//La manera de interpretar el mensaje es diferente
        }
    }

    protected override void ButtonListo()
    {
        base.ButtonListo();
    }

    protected override void ButtonInfo()
    {
        base.ButtonInfo();
    }

    protected override void AfterInstructions()
    {
        instruccionsInGame.SetActive(true);
        buttonInfo.enabled = false;
        buttonInfo.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttonListo.gameObject);
    }


    //Override methods END

    //MissionCafeteria Methods BEGINING

    private void ButtonPagar()
    {
        //ACTIVAR 

        openAI.GetComponent<ManagerOpenAI>().listener.buttonResponder.gameObject.SetActive(false);
        openAI.GetComponent<ManagerOpenAI>().messageText.text = "Muchas gracias, ¡buen día!";
        buttonPagar.gameObject.SetActive(false);
        
        buttonInfo.gameObject.SetActive(false);
        Invoke("CloseChat", 2.0f);
    }


    //En esta mision queremos analizar el comportamiento del usuario cuando le ocurre algo inesperado.
    //En éste caso, que el camarero se equivoque al escuchar la orden
    //Gracias al promt (Ver en la jerarquía -> MisionCafeteriaFather -> MissionCafeteria -> OpenAIManager -> En el Inpector busca "Promt")
    //Podemos trackear por donde va la interaccion
    private void ProcessInteraction(KeyInteraction interaction, string chatGPTMessage) 
    {
        // Verificar si la interacción se completa con el mensaje actual
        interaction.CheckCompletion(chatGPTMessage);

        if (!interaction.IsCompleted || interaction.HasBeenNotified)
            return;

        ChangeInstructions(interaction.Instruction);

        if (ContainsKeyword(chatGPTMessage, "el total es"))//Ponemos una "contraseña" en el promt para saber que ocurre
        {
            buttonPagar.gameObject.SetActive(true);
        }

        if (MessageContainsApology(chatGPTMessage))
        {
            MarkInteractionAsCompleted(interaction, chatGPTMessage);
        }
        else
        {
            // Revertir si no hay disculpa
            interaction.IsCompleted = false;
        }
    }

    private bool ContainsKeyword(string message, string keyword)
    {
        return message.IndexOf(keyword, System.StringComparison.OrdinalIgnoreCase) != -1;
    }

    private bool MessageContainsApology(string message)
    {
        return apologyKeywords.Any(apology => ContainsKeyword(message, apology));
    }

    private void MarkInteractionAsCompleted(KeyInteraction interaction, string chatGPTMessage)
    {
        stats.UpdateStatImage(interaction.InteractionName);

        if (ContainsKeyword(chatGPTMessage, "el total es"))
        {
            textInstrucciones.text = "Pulsa el botón 'PAGAR' para acabar la misión";
            openAI.GetComponent<ManagerOpenAI>().listener.buttonResponder.gameObject.SetActive(false);
        }

        interaction.HasBeenNotified = true;
    }

    //MissionCafeteria Methods END

}
