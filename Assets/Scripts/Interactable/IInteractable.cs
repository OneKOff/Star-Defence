using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void CheckTriggerEnter(Collider2D other);
    void CheckTriggerStay(Collider2D other);
    void CheckTriggerExit(Collider2D other);
}
