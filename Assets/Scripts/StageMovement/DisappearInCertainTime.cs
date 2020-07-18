using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DisappearInCertainTime : MonoBehaviour
{
    [SerializeField] private float disappearTimeFromAppear = 5f;
    [SerializeField] private float appearTimeFromDisappear = 2f;

    private Material _material;
    private Color defaultColor;

    private void Awake()
    {
        this._material = GetComponent<MeshRenderer>().material;
        this.defaultColor = this._material.color;
    }
}
