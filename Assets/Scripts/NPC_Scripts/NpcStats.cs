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
            new KeyInteraction("Nombre", new List<string>{nombre},"Pregunta por sus intereses:\n*�Qu� estudias?\n*�Qu� tipo de m�sica escuchas?\n*�Haces alg�n deporte?\n*�Qu� haces en tu tiempo libre?\n*�Qu� tipo de series o pel�culas te gustan?\n*�Tienes mascotas?"),
            new KeyInteraction("Estudios", estudios ,"Pregunta por sus intereses:\n*�Qu� estudias?\n*�Qu� tipo de m�sica escuchas? \n*�Haces alg�n deporte?\n*�Qu� haces en tu tiempo libre?\n*�Qu� tipo de series o pel�culas te gustan?\n*�Tienes mascotas?"),
            new KeyInteraction("Musica", musica,"Pregunta por sus intereses:\n*�Qu� estudias?\n*�Qu� tipo de m�sica escuchas?\n*�Haces alg�n deporte?\n*�Qu� haces en tu tiempo libre?\n*�Qu� tipo de series o pel�culas te gustan?\n*�Tienes mascotas?"),
            new KeyInteraction("Deporte", deporte,"Pregunta por sus intereses:\n*�Qu� estudias?\n*�Qu� tipo de m�sica escuchas?\n*�Haces alg�n deporte?\n*�Qu� haces en tu tiempo libre?\n*�Qu� tipo de series o pel�culas te gustan?\n*�Tienes mascotas?"),
            new KeyInteraction("Pasatiempo", pasatiempo.Concat(peliOSerie).ToList(),"Pregunta por sus intereses:\n*�Qu� estudias?\n*�Qu� tipo de m�sica escuchas?\n*�Haces alg�n deporte?\n*�Qu� haces en tu tiempo libre?\n*�Qu� tipo de series o pel�culas te gustan?\n*�Tienes mascotas?"),
            new KeyInteraction("Mascota", new List<string>{mascota},"Pregunta por sus intereses:\n*�Qu� estudias?\n*�Qu� tipo de m�sica escuchas?\n*�Haces alg�n deporte?\n*�Qu� haces en tu tiempo libre?\n*�Qu� tipo de series o pel�culas te gustan?\n*�Tienes mascotas?"),
            new KeyInteraction("Preguntar por el usuario",new List<string>{" tu"," te"},"Da respuestas claras que motiven a seguir la conversaci�n:\n*'Yo ... �y t� ...?")
        };
    }

    public List<KeyInteraction> GetKeyInteractions()
    {
        return keyInteractions;
    }
}
