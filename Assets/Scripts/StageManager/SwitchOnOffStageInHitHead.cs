using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnOffStageInHitHead : MonoBehaviour
{
    private MeshRenderer _renderer;

    private void Awake()
    {
        this._renderer = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitHeadCheck"))
        {
            SwitchOnOffStageInHitHeadController.Instance.SwitchAll();
        }
        if (other.CompareTag("PlayerGroundCheck"))
        {
            if(other.transform.parent.GetComponent<PlayerMovementBasedCamera>().IsHipDropping)
            {
                SwitchOnOffStageInHitHeadController.Instance.SwitchAll();
            }
        }
    }

    public void Switch(Material toChangeMaterial)
    {
        this._renderer.material = toChangeMaterial;
    }
}
