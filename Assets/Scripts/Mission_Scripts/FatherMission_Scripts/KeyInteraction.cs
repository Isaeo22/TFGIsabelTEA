using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Representa una interacción clave paras la misión.
/// La clase maneja el estado de la interacción y verifica si el mensaje del usuario o la IA cumple con los requisitos para completarla.
/// </summary>
public class KeyInteraction 
{
    public string InteractionName { get; set; }
    public List<string> RequiredKeywords { get; set; }
    public bool IsCompleted { get;  set; }
    public string Instruction { get; set; }
    public bool HasBeenNotified { get; set; } // Indica si la misión ya ha sido notificada al completar.



    public KeyInteraction(string interactionName,List<string> requiredkeyWords, string instruction)
    {
        InteractionName = interactionName;
        RequiredKeywords = requiredkeyWords;
        IsCompleted = false;
        Instruction = instruction;
        HasBeenNotified = false;
    }

    public void CheckCompletion(string message)
    {
        foreach (var keyword in RequiredKeywords)
        {
            if (CompareSentences(message, keyword))
            {
                IsCompleted = true;
                break; 
            }
        }
    }


    bool CompareSentences(string message, string word) 
    {
        return message.IndexOf(word, System.StringComparison.OrdinalIgnoreCase) != -1;
    }
}
