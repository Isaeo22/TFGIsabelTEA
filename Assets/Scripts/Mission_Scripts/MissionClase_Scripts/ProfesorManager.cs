using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ProfesorManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    

    [SerializeField] TextMeshProUGUI pizarra;

    private bool stopRequested = false; // Nueva variable para controlar la detención
    public void PlayAudio(AudioClip a)
    {
        if (stopRequested) return; // Si se solicitó detener, no reproducir nada
        audioSource.clip = a;
        audioSource.Play();    

    }

  

    public void PlaySinglePaso(PasoClase paso, Action onComplete = null)
    {
        if (stopRequested) return; // Si se solicitó detener, no reproducir nada
        PlayAudio(paso.audio);
       

        // Actualizar el texto en pantalla (si aplica)
        ActualizarPizarra(paso.texto); 
    }

    public void PlayPasoList(List<PasoClase> pasos, Action onComplete = null)
    {
        if (pasos == null || pasos.Count == 0)
        {
            Debug.LogWarning("La lista de pasos está vacía o es nula.");
            
            return;
        }
        stopRequested = false;
        StartCoroutine(PlayAudioListCoroutine(pasos,onComplete));
    }

    private IEnumerator PlayAudioListCoroutine(List<PasoClase> pasos, Action onComplete)
    {
        foreach (PasoClase p in pasos)
        {
            if (stopRequested) yield break; // Si se solicitó detener, no reproducir nada
            if (p.audio != null)
            {
                PlayAudio(p.audio);
                

                // Actualizar el texto asociado al audio
                ActualizarPizarra(p.texto);

                // Espera a que el clip termine de reproducirse
                yield return new WaitForSeconds(p.audio.length);
            }
            else
            {
                Debug.LogWarning("El audio en uno de los pasos es nulo.");
            }
        }

        onComplete?.Invoke();
    }
    private void ActualizarPizarra(string t)
    {
        if (t != "")
        {
            pizarra.text = t;
        }
    }

    public void StopAudio()
    {
        stopRequested = true;
        audioSource.Stop();
      StopAllCoroutines(); // Detiene cualquier corrutina en ejecución
        
    }

}
