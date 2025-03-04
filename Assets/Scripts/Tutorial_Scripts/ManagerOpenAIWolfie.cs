using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerOpenAIWolfie : MonoBehaviour
{
    [SerializeField] TalkerOpenAI wolfie;
  
    List<string> dialogueWolfie;
    public void Awake()
    {
        wolfie.rolText.text = "Wolfie";
        dialogueWolfie = new List<string> {
        "Hola! Tu debes ser "+ PlayerPrefs.GetString("username")+",\nEncantado, yo soy Wolfie",
        "Bienvenido al juego de entrenamiento 'Mi primer día de universidad'\n¿Quieres ver el tutorial?",
        "¿Estás seguro?",
        "De acuerdo, si está todo claro vamos a empezar"
    };
        

    }
    public void NextTutorial(int step)
    {
        wolfie.message.text = dialogueWolfie[step];
    }
}
