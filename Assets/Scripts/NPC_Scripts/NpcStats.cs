using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Maneja las interacciones claves de los npcs de MisionAmigos
/// </summary>
public class NpcStats : MonoBehaviour
{
    
    [SerializeField]  public string nombre;
    [SerializeField]  public List<string> estudios;
    [SerializeField] public List<string> musica;
    [SerializeField] public List<string> deporte;
    [SerializeField] public List<string> pasatiempo;
    [SerializeField] public List<string> peliOSerie;
    [SerializeField] public string mascota;

    private List<KeyInteraction> keyInteractions;

    private void Start()
    {
        InitializeOpenAiKeyInteractions();  //inicializa las interacciones siguiendo la informacion de cada uno
    }
    private void InitializeOpenAiKeyInteractions()
    {
         keyInteractions=new List<KeyInteraction>
        {
            new KeyInteraction("Nombre", new List<string>{nombre},"Pregunta por sus intereses:\n*¿Qué estudias?\n*¿Qué tipo de música escuchas?\n*¿Haces algún deporte?\n*¿Qué haces en tu tiempo libre?\n*¿Qué tipo de series o películas te gustan?\n*¿Tienes mascotas?"),
            new KeyInteraction("Estudios", estudios ,"Pregunta por sus intereses:\n*¿Qué estudias?\n*¿Qué tipo de música escuchas? \n*¿Haces algún deporte?\n*¿Qué haces en tu tiempo libre?\n*¿Qué tipo de series o películas te gustan?\n*¿Tienes mascotas?"),
            new KeyInteraction("Musica", musica,"Pregunta por sus intereses:\n*¿Qué estudias?\n*¿Qué tipo de música escuchas?\n*¿Haces algún deporte?\n*¿Qué haces en tu tiempo libre?\n*¿Qué tipo de series o películas te gustan?\n*¿Tienes mascotas?"),
            new KeyInteraction("Deporte", deporte,"Pregunta por sus intereses:\n*¿Qué estudias?\n*¿Qué tipo de música escuchas?\n*¿Haces algún deporte?\n*¿Qué haces en tu tiempo libre?\n*¿Qué tipo de series o películas te gustan?\n*¿Tienes mascotas?"),
            new KeyInteraction("Pasatiempo", pasatiempo.Concat(peliOSerie).ToList(),"Pregunta por sus intereses:\n*¿Qué estudias?\n*¿Qué tipo de música escuchas?\n*¿Haces algún deporte?\n*¿Qué haces en tu tiempo libre?\n*¿Qué tipo de series o películas te gustan?\n*¿Tienes mascotas?"),
            new KeyInteraction("Mascota", new List<string>{mascota},"Pregunta por sus intereses:\n*¿Qué estudias?\n*¿Qué tipo de música escuchas?\n*¿Haces algún deporte?\n*¿Qué haces en tu tiempo libre?\n*¿Qué tipo de series o películas te gustan?\n*¿Tienes mascotas?"),
            new KeyInteraction("Preguntar por el usuario",new List<string>{" tu"," te"},"Da respuestas claras que motiven a seguir la conversación:\n*'Yo ... ¿y tú ...?")
        };
    }

    public List<KeyInteraction> GetKeyInteractions()
    {
        return keyInteractions;
    }
}
