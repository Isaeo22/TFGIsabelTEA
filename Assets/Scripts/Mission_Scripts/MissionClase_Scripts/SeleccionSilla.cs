using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeleccionSilla : Seleccion
{

    [Header("Selección de silla")]   
    [SerializeField] List<Button> buttons;
    [SerializeField] GameObject buttonsGO;
    [SerializeField] List<GameObject> flechas;
    private Dictionary<GameObject, int> buttonIndexLookup;

    GameObject flechaInstance;
    GameObject currentSelectedButton;
    GameObject lastSelectedButton;
    Silla actualSilla;
    bool sillaSeleccionada;
    [SerializeField]List<GameObject> sillas;
    int index = 0;
    float speed;

    //Delegados 
    public event Action<Silla> OnEndSilla;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;

        foreach (Button b in buttons)
        {
            b.onClick.AddListener(ButtonElegirSilla);
        }

          // Inicializa índices de botones
        buttonIndexLookup = new Dictionary<GameObject, int>();
        for (int i = 0; i < buttons.Count; i++)
        {
            buttonIndexLookup[buttons[i].gameObject] = i;
        }
    }

    public override void Restart()
    {
        buttonsGO.SetActive(true);
        sillaSeleccionada = false;
        index = 0;
        base.Restart();
    }

    // Update is called once per frame
    void Update()
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

        if (sillaSeleccionada)
        {
            buttonInfo.gameObject.SetActive(false);
            var step = speed * Time.deltaTime;
            Vector3 pos = Vector3.MoveTowards(GameManager.Instance.GetPlayerTransform().position, actualSilla.talkingPosition, step);


            GameManager.Instance.SetPlayerTransform(pos, GameManager.Instance.GetPlayerTransform().rotation);


            if (GameManager.Instance.GetPlayerTransform().position == actualSilla.talkingPosition)
            {
                sillaSeleccionada = false;
                
                OnEndSilla?.Invoke(actualSilla);
                buttonInfo.gameObject.SetActive(true);
            }

        }
    }

    void ChangeSelectedButton(int i)
    {
        flechaInstance = flechas[i];
        flechaInstance.SetActive(true);
        index = i;
    }
    public void ActivarSeleccionSilla()
    {
        //actualUI = seleccionDeSilla;
        this.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        textoSeleccion.text = "¿Dónde te quieres sentar?";
        textoSeleccionGO.SetActive(true);  
       
    }

    void ButtonElegirSilla()
    {
        actualSilla = sillas[index].GetComponent<Silla>();
        buttonsGO.SetActive(false);

        sillaSeleccionada = true;


        textoSeleccionGO.SetActive(false);


        buttonInfo.enabled = false;
        flechaInstance.SetActive(false);
    }
  
}
