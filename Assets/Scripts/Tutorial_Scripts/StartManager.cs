using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] Button buttonEmpezar;
    [SerializeField] Button buttonOpciones;
    [SerializeField] PlayerInfoManager playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        buttonEmpezar.onClick.AddListener(ButtonEmpezar);
        Cursor.visible = false; // Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla

    }

    void ButtonEmpezar()
    {
        this.gameObject.SetActive(false);
        playerInfo.StartPlayerInfo();
    }
    void ButtonOpciones()
    {
        Debug.Log("Mostrar Opciones");
    }

    
}
