using System;
using UnityEngine;

public class PortaController : MonoBehaviour
{
    public Animator anim;
    private bool isOpen;
    private bool isInteractable;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isInteractable)
        {
            InteractOM.OnInteract += OpenClose;
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InteractOM.OnInteract -= OpenClose;
        }
    }

    private void OpenClose()
    {
        if (!isOpen)
        {
            anim.Play("PortaAbrindo");
            isOpen = true;
        }
    }
}