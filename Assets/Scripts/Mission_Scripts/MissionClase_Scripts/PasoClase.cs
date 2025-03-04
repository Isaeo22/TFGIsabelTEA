using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PasoClase
{
    public AudioClip audio;  // Clip de audio del paso
    public string texto;     // Texto asociado al audio
    

    public PasoClase(AudioClip a, string t)
    {
        this.audio = a;
        this.texto = t;
    }
}