using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToUpStageCheckPoint : MonoBehaviour
{
    private BoxCollider _boxCollider;

    private void Awake()
    {
        this._boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this._boxCollider.isTrigger = false;
        }
    }
}
