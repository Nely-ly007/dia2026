using System;
using UnityEngine;

public class InteractOM
{
    public static event Action OnInteract;

    public static void Interact()
    {
        OnInteract?.Invoke();
    }
    
}
