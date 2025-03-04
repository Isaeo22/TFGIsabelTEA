using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Script para los GameObjects a los que debe mirar el jugador
/// Detecta cuanto tiempo mantiene contacto visual
/// </summary>
public class EyetrackerObject : MonoBehaviour
{
    [SerializeField] bool onVr;

    GazeAware gazeAware;//Componente para el eyetracker
 
    public Transform head; // Referencia a la cabeza (XR Camera)
    Transform target; // El objeto al que queremos mirar
    public float allowedAngle = 10f; // Margen de error en grados

     //VARIABLES JSON
     float totalTime;
     float eyetrackingTime;
     float totalEyetrackingTime;

     bool startCrono = false;
   

    public void Crono()
    {
        totalTime += Time.deltaTime;

    }

    public void PlayCrono()
    {
        startCrono = true;
        
    }
    public void PauseCrono()
    {
        startCrono=false;
    }
    void Start()
    {       
        gazeAware = GetComponent<GazeAware>();
        target = this.transform;
    }

    
    void Update()
    {
        if (!onVr) //Si no estamos en VR contabilizamos con el eyetracker
        {
            if (gazeAware.HasGazeFocus)
            {
                totalEyetrackingTime += Time.deltaTime;
            }
            if (startCrono)
            {

                if (gazeAware.HasGazeFocus)
                {
                    eyetrackingTime += Time.deltaTime;
                }
                Crono();
            }
        }
        else //Si estamos en VR contabilizamos con el angulo de la camara
        {

            
            Vector3 direccionAlObjeto = (target.position - head.position).normalized;
            float angulo = Vector3.Angle(head.forward, direccionAlObjeto);

                         
           
            if (angulo < allowedAngle)
            {
                totalEyetrackingTime += Time.deltaTime;
               
            }
            if (startCrono)
            {

                if (angulo < allowedAngle)
                {
                    eyetrackingTime += Time.deltaTime;
                    Debug.Log("Pillando eyetracking bien");
                }
                Crono();
            }
        }
      
     


    }

    public float GetEyetrackingPercentage()
    {
        if (totalTime == 0)
        {
            return 0f;
        }
        // Calcular el porcentaje de tiempo mirando el objeto
        float percentage = (eyetrackingTime / totalTime) * 100f;
        Debug.Log("Porcentaje de tiempo mirando: " + percentage + "%");
        return percentage;
    }

    public float GetTotalEyetrackingTime()
    {
        return totalEyetrackingTime;
    }

    public void Reestart()
    {
        totalTime=0;
        eyetrackingTime=0;
        totalEyetrackingTime=0;
    }

}
