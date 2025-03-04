using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
///Maneja la seleccion de personaje de la Mision Amigos
///Se encarga de preguntar al usuario con quien quiere hablar
/// </summary>
public class SeleccionDePersonaje : MonoBehaviour
{
    [Header("UI Selección")]
    [SerializeField] GameObject seleccionDePersonaje;
    [SerializeField] TextMeshProUGUI textoSeleccion;

    [SerializeField] GameObject buttonsGO;
    [SerializeField] GameObject buttonsSiNo;
    [SerializeField] Button buttonSi;
    [SerializeField] Button buttonNo;

    [Header("Personajes")]
    [SerializeField] List<GameObject> npcs;
    [SerializeField] List<GameObject> buttons;
    [SerializeField] List<GameObject> flechas;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject canvasStats;

    Vector3 initialCanvasPos;
    Quaternion initialCanvasRot;

    private GameObject currentSelectedButton;
    private GameObject lastSelectedButton;
    private GameObject flechaInstance;
    private int index = -1;
    private NpcManager actualNpc;
    private Dictionary<GameObject, int> buttonIndexLookup;

    
    private Vector3 initialSpawnPosition;
    private Quaternion initialSpawnRotation;

   //Delegado para pasar el NPC seleccionado
    public event Action<NpcManager> OnNpcSelected;
  
    private void Awake()
    {
        initialCanvasPos = canvas.transform.position;
        initialCanvasRot = canvas.transform.rotation;

         // Establecemos la posición y rotación inicial del jugador
         initialSpawnPosition = GameManager.Instance.GetPlayerTransform().position;
        initialSpawnRotation = GameManager.Instance.GetPlayerTransform().rotation;


        buttonSi.onClick.AddListener(ButtonSi);
        buttonNo.onClick.AddListener(ButtonNo);

        // Inicializa índices de botones
        buttonIndexLookup = new Dictionary<GameObject, int>();
        for (int i = 0; i < buttons.Count; i++)
        {
            buttonIndexLookup[buttons[i]] = i;
        }

        // Agrega listeners a los botones
        foreach (GameObject b in buttons)
        {
            b.GetComponent<Button>().onClick.AddListener(ButtonElegirNPC);
        }
    }

    private void Update()
    {
        // Asegurarse de que buttonIndexLookup esté inicializado
        if (buttonIndexLookup == null || buttons.Count == 0) return;
        // Obtener el botón seleccionado actualmente solo una vez por actualización
        currentSelectedButton = EventSystem.current.currentSelectedGameObject;


        if (currentSelectedButton != lastSelectedButton && currentSelectedButton != null)
        {
            lastSelectedButton = currentSelectedButton;
            //Debug.Log(currentSelectedButton);


            if (flechaInstance != null)
            {
                flechaInstance.SetActive(false);
            }

            // Comprobar si el botón seleccionado está en el diccionario
            if (buttonIndexLookup.TryGetValue(currentSelectedButton, out int newIndex))
            {
                ChangeSelectedButton(newIndex);
            }


        }
    }

    public void ActivarSeleccion()
    {
        seleccionDePersonaje.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        textoSeleccion.text = "¿Con quién quieres hablar?";
   
    }

    private void ChangeSelectedButton(int i)
    {
        flechaInstance = flechas[i];
        flechaInstance.SetActive(true);
        index = i;
    }

    private void ButtonElegirNPC()
    {
        actualNpc = npcs[index].GetComponent<NpcManager>();
        buttonsGO.SetActive(false);
        textoSeleccion.text = "¿Quieres hablar con " + actualNpc.gender + "?";

        // Cambiar posición del jugador
        GameManager.Instance.SetPlayerTransform(actualNpc.talkingPosition, actualNpc.talkingRotation);
        // Posicionar el Canvas frente al jugador
        canvas.transform.position = actualNpc.talkingPosition + GameManager.Instance.GetPlayerTransform().forward * 1.5f + GameManager.Instance.GetPlayerTransform().up * 0.3f;

        // Hacer que el Canvas mire al jugador, pero solo en el eje Y
        Vector3 directionToPlayer = GameManager.Instance.GetPlayerTransform().position - canvas.transform.position;
        directionToPlayer.y = 0; // Mantener la rotación en el eje Y, sin afectar el eje X

        // Ajustar la rotación solo en el eje Y
        canvas.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        canvas.transform.Rotate(0, 180, 0);
        buttonsSiNo.SetActive(true);
    }

    private void ButtonSi()
    {
        seleccionDePersonaje.SetActive(false);
        buttonsSiNo.SetActive(false);
        buttonsGO.SetActive(true);

        actualNpc = npcs[index].GetComponent<NpcManager>(); // Obtener el NPC seleccionado

        canvasStats.transform.position = actualNpc.talkingPosition + GameManager.Instance.GetPlayerTransform().forward * 1.5f + GameManager.Instance.GetPlayerTransform().up * 0.3f;

        // Hacer que el Canvas mire al jugador, pero solo en el eje Y
        Vector3 directionToPlayer = GameManager.Instance.GetPlayerTransform().position - canvasStats.transform.position;
        directionToPlayer.y = 0; // Mantener la rotación en el eje Y, sin afectar el eje X

        // Ajustar la rotación solo en el eje Y
        canvasStats.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        canvasStats.transform.Rotate(0, 180, 0);
        

        OnNpcSelected?.Invoke(actualNpc); // Notifica al controlador de misión
    }

    private void ButtonNo()
    {
        seleccionDePersonaje.SetActive(true);
        buttonsSiNo.SetActive(false);

        // Restaurar la posición inicial del jugador
        GameManager.Instance.SetPlayerTransform(initialSpawnPosition, initialSpawnRotation);
        canvas.transform.SetPositionAndRotation(initialCanvasPos,initialCanvasRot);
        
        buttonsGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buttons[index]);
        textoSeleccion.text = "¿Con quién quieres hablar?";
    }

    public void RestartSelection()
    {
        seleccionDePersonaje.SetActive(true);
        textoSeleccion.text = "¿Con quién quieres hablar?";

        
            actualNpc.openAI.SetActive(false);
        
        GameManager.Instance.SetPlayerTransform(initialSpawnPosition, initialSpawnRotation);
        GameManager.Instance.SetPlayerTransform(initialSpawnPosition, initialSpawnRotation);
        canvas.transform.SetPositionAndRotation(initialCanvasPos, initialCanvasRot);
        
        //canvas.transform.SetPositionAndRotation(initialCanvasPos.position, initialCanvasPos.rotation);
        //transform.Rotate(0, -180, 0);
        buttons[index].SetActive(false);
    }

    public void EndSelection()
    {
        textoSeleccion.text = "Hablaste con todos, ¡Muy bien!";
       
    }

    public void RestartMision()
    {
        foreach (GameObject b in buttons)
        {
            b.SetActive(true);
        }
        seleccionDePersonaje.SetActive(false);

        if (actualNpc != null)
        {
            actualNpc.openAI.SetActive(false);
        }

       
            canvas.transform.SetPositionAndRotation(initialCanvasPos, initialCanvasRot);
        
       
    }
    public int NpcsCount => npcs.Count;
}