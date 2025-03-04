using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaseExplicacion : MonoBehaviour
{
    public List<PasoClase> explicacion;
    public List<PasoClase> reexplicacion;
    [SerializeField] List<PasoClase> empezarAudio;
    [SerializeField] ProfesorManager profesor;

    [SerializeField] GameObject levantarManoDudas;
    [SerializeField] Button noLevantarMano;
    [SerializeField] Button levantarMano;

    public event Action<bool> OnEndExplicacion;
    public event Action<string> OnInfoUpDate;
    bool preguntoDudas;

    private void Start()
    {
        noLevantarMano.onClick.AddListener(NoLevantarMano);
        levantarMano.onClick.AddListener(LevantarMano);
    }
    public void Restart()
    {
        profesor.StopAudio();
        levantarManoDudas.SetActive(false);
        preguntoDudas = false;
    }
    public void ActivarClaseExplicacion()
    {
        this.gameObject.SetActive(true);
        profesor.PlayPasoList(explicacion, () => FinalizarExplicacion()); 
        
    }
    public void FinalizarExplicacion()
    {
        levantarManoDudas.SetActive(true);
        OnInfoUpDate?.Invoke("Levanta la mano si no has entendido la explicación");

    }
    public void FinalizarReexplicacion()
    {
        OnInfoUpDate?.Invoke("Levanta la mano para tener el turno de palabra");
        this.gameObject.SetActive(false);
        OnEndExplicacion?.Invoke(preguntoDudas);
    }

    public void NoLevantarMano()
    {
     
        preguntoDudas =false;
        levantarManoDudas.SetActive(false);
        OnInfoUpDate?.Invoke("Levanta la mano para tener el turno de palabra");
        profesor.PlayPasoList(empezarAudio,() => FinalizarReexplicacion());       
    }

    public void LevantarMano()
    {
        OnInfoUpDate?.Invoke("Atiende a la explicación del profesor");
        preguntoDudas =true;
        levantarManoDudas.SetActive(false);
        profesor.PlayPasoList(reexplicacion, () => FinalizarReexplicacion());
        
    }
}
