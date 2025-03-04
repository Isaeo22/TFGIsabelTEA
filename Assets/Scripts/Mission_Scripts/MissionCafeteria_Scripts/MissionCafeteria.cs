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
    [SerializeField] GameObject npcsGameObject;//Npcs dentro de la cafeter�a
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
        missionName = "Misi�n Cafeter�a";

        stats.InitializeMissionDictionary();

        json.SetManagerOpenAI(openAI.GetComponent<ManagerOpenAI>());

        ReestartStats();
        textInstrucciones.text = "Inicia la conversaci�n con un saludo:\n * Hola / Buenos d�as";
        openAI.GetComponent<ManagerOpenAI>().ReestartManagerOpenAI();
        buttonPagar.gameObject.SetActive(false);

        //START MISION CAFETERIA
        buttonPagar.onClick.AddListener(ButtonPagar);
        apologyKeywords = new List<string> { "lo siento", "disculpa", "perdona", "perd�n" };
        npcsGameObject.SetActive(true);

        //Llamar a la clase base
        base.StartMission();
    }

    protected override void InitializeUserKeyInteractions()
    {
        keyUserInteractions = new List<KeyInteraction>
        {
             new KeyInteraction("Saludo", new List<string> { "hola", "buenos d�as" }, "Mira el men�, elige algo y p�delo:\n *Un caf� y un donut por favor.\n No te olvides de pedir por favor y dar las gracias"),
             new KeyInteraction("Despedida", new List<string> { "hasta luego", "adi�s" },null),
             new KeyInteraction("Por favor", new List<string> { "por favor" }, null),
             new KeyInteraction("Gracias", new List<string> { "gracias" }, null),
             new KeyInteraction("Pedir del Menu",  new List<string> { "bocadillo", "hamburguesa", "pizza", "bebida", "batido", "caf�", "donut", "patatas", "helado" }
            , "Al pedir aseg�rate de que el camarero escucha bien la orden,si no, corr�gele:\n*Perdona, eso no es lo que he pedido"),
             
        };
    }
    protected override void InitializeOpenAiKeyInteractions()
    {
        keyOpenAiInteractions = new List<KeyInteraction>
        {  
            new KeyInteraction("Corregir precio", new List<string> { "euro", "�", "precio" },
                  "Comprueba el precio de lo que has pedido antes de pagar: \n *Perdona, en el men� no pone ese precio"),

            new KeyInteraction("Corregir orden", new List<string> { "bocadillo", "hamburguesa", "pizza", "bebida", "batido", "caf�", "donut", "patatas", "helado" }, null),
            
        };
    }

    //Override methods BEGINING
 
    protected override void AnalyzeUserMessage()
    {
        string userMessage = GetUserMessage();
        foreach(var interaction in keyUserInteractions)
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
        openAI.GetComponent<ManagerOpenAI>().messageText.text = "Muchas gracias, �buen d�a!";
        buttonPagar.gameObject.SetActive(false);
        
        buttonInfo.gameObject.SetActive(false);
        Invoke("CloseChat", 2.0f);
    }


    //En esta mision queremos analizar el comportamiento del usuario cuando le ocurre algo inesperado.
    //En �ste caso, que el camarero se equivoque al escuchar la orden
    //Gracias al promt (Ver en la jerarqu�a -> MisionCafeteriaFather -> MissionCafeteria -> OpenAIManager -> En el Inpector busca "Promt")
    //Podemos trackear por donde va la interaccion
    private void ProcessInteraction(KeyInteraction interaction, string chatGPTMessage) 
    {
        // Verificar si la interacci�n se completa con el mensaje actual
        interaction.CheckCompletion(chatGPTMessage);

        if (!interaction.IsCompleted || interaction.HasBeenNotified)
            return;

        ChangeInstructions(interaction.Instruction);

        if (ContainsKeyword(chatGPTMessage, "el total es"))//Ponemos una "contrase�a" en el promt para saber que ocurre
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
            textInstrucciones.text = "Pulsa el bot�n 'PAGAR' para acabar la misi�n";
            openAI.GetComponent<ManagerOpenAI>().listener.buttonResponder.gameObject.SetActive(false);
        }

        interaction.HasBeenNotified = true;
    }

    //MissionCafeteria Methods END

}
