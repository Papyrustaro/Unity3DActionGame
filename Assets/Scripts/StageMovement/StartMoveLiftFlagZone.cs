using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMoveLiftFlagZone : MonoBehaviour
{
    [SerializeField] private KeepMoveUntilDestination moveLift;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.moveLift.CanMove = true;
        }
    }
}
