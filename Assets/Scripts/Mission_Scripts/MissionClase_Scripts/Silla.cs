using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silla : MonoBehaviour
{
    [SerializeField] public int indexButton;

    [Header("Player talking Position")]

    public Vector3 talkingPosition;

    public Vector3 sittingPosition;
 
 

    [SerializeField]public string item;
    [SerializeField]public AudioClip clip;

    [SerializeField] public GameObject npc;
    [SerializeField] public GameObject row1Item;
    [SerializeField] public GameObject row2Item;


 
}
