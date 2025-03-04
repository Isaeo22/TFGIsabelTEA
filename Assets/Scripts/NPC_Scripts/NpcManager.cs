using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] public GameObject openAI;
    private Animator animator;
    private float timer;
    float timeBetweenAnimations;  // Tiempo en segundos entre cambios de animación

    [Header("Player talking Position")]
    [SerializeField] Transform playerTalkingTransform;
    [HideInInspector]
    public Vector3 talkingPosition;
    [HideInInspector]
    public Quaternion talkingRotation;

    [Header("Info NPC")]
    [SerializeField] public string nombre;
    [SerializeField] public string gender;



    [SerializeField] public GameObject head;

    [SerializeField]public int indexButton;

  

    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeAnimation();
        talkingPosition = playerTalkingTransform.position;
        talkingRotation = playerTalkingTransform.rotation;
    
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAnimations)
        {
            ChangeAnimation();
            timer = 0f;
        }
    }

    void ChangeAnimation()
    {
        int randomIndex = Random.Range(0, 3); // Genera un número aleatorio entre 0 y 2
        animator.SetInteger("RandomInt", randomIndex);
    }
}
