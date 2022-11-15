using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_WorldInteractable
{
    void OnButtonUp();
    void OnButtonHeld();
    void OnButtonDown();
    bool IsInteractable();
    void Highlight();
    void Dehighlight();
}
