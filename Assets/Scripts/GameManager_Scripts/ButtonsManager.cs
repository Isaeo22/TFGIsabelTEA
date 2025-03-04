using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Administra la selección automática de botones en la UI.
/// Garantiza que siempre haya un botón seleccionado en el EventSystem.
/// </summary>
public class ButtonsManager : MonoBehaviour
{
    public EventSystem EventSystem;
    public List<Button> buttons = new List<Button>();
    

    private void Start()
    {
        EventSystem = FindObjectOfType<EventSystem>(true);
        GetChildSelectables();
    }
    private void Update()
    {
        //Si hay un boton activo en la escena y seleccionado no hace nada
        if (EventSystem.currentSelectedGameObject != null && EventSystem.currentSelectedGameObject.activeInHierarchy)
        {
        
            return;
        }
        else //Si no hay un boton seleccionado elimina los botones nulos (Pausa) y selecciona el primer boton activo
        {
            buttons.RemoveAll(item => item == null);
            foreach (var item in buttons)
            {
             
                if (item.isActiveAndEnabled)
                {
                    EventSystem.SetSelectedGameObject(item.gameObject);
                
                    return;
                }

            }
        }


    }


    /// <summary>
    /// Busca y almacena todos los botones en la escena.
    /// </summary>
    public void GetChildSelectables()
    {
        buttons.Clear();
        buttons.AddRange(FindObjectsOfType<Button>(true));
    }
}

