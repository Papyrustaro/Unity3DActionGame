using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// onOffブロックによって、消えたり現れたりする
/// </summary>
public class SwitchOnOffStage : MonoBehaviour
{
    [SerializeField] private bool initOn = true;
    [SerializeField, ReadOnly] private Material initOnInOnMaterial;
    [SerializeField, ReadOnly] private Material initOnInOffMaterial;
    [SerializeField, ReadOnly] private Material initOffInOnMaterial;
    [SerializeField, ReadOnly] private Material initOffInOffMaterial;

    [SerializeField, ReadOnly]
    private bool currentOn = true;
    [SerializeField, ReadOnly]
    private MeshRenderer _meshRenderer;
    [SerializeField, ReadOnly]
    private BoxCollider _collider;

    public void SwitchOnOff()
    {
        if (this.currentOn)
        {
            this.currentOn = false;
            this._collider.enabled = false;
            if (this.initOn) this._meshRenderer.material = this.initOnInOffMaterial;
            else this._meshRenderer.material = this.initOffInOffMaterial;
        }
        else
        {
            this.currentOn = true;
            this._collider.enabled = true;
            if (this.initOn) this._meshRenderer.material = this.initOnInOnMaterial;
            else this._meshRenderer.material = this.initOffInOnMaterial;
        }
    }

    public void InitSet(Material initOnInOnMaterial, Material initOnInOffMaterial, Material initOffInOnMaterial, Material initOffInOffMaterial)
    {
        this.initOnInOnMaterial = initOnInOnMaterial;
        this.initOnInOffMaterial = initOnInOffMaterial;
        this.initOffInOnMaterial = initOffInOnMaterial;
        this.initOffInOffMaterial = initOffInOffMaterial;
        this._meshRenderer = GetComponent<MeshRenderer>();
        this._collider = GetComponent<BoxCollider>();
        if (this.initOn) this._meshRenderer.material = this.initOnInOnMaterial;
        else
        {
            this.currentOn = false;
            this._meshRenderer.material = this.initOffInOffMaterial;
            this._collider.enabled = false;
        }
    }
}
