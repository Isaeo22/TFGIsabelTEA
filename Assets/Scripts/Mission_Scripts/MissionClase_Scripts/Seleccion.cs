using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Seleccion : MonoBehaviour
{

    [SerializeField] protected GameObject keywordRecognizer;
    [SerializeField] protected GameObject textoSeleccionGO;
    [SerializeField] protected TextMeshProUGUI textoSeleccion;

    [SerializeField] protected Button buttonInfo;
    public virtual void Restart()
    {
        textoSeleccionGO.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
