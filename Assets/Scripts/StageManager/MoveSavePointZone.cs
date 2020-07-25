using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSavePointZone : MonoBehaviour
{
    [SerializeField] private StageSavePoint savePoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.savePoint.WarpPoint();
        }
    }
}
