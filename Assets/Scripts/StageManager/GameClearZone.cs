using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.StageClear();
        }
    }
}
