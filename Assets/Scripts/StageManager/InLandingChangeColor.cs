using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーが接地しているときに、接地しているgameObjectのカラーを変える
/// </summary>
public class InLandingChangeColor : MonoBehaviour
{
    [SerializeField] private Color InNotLandingColor = Color.white;
    [SerializeField] private Color InLandingColor = Color.white;
    
    [SerializeField] private UnityEvent onGrounded;
    [SerializeField] private UnityEvent onLeaveFromGround;

    private MeshRenderer _renderer;

    private void Awake()
    {
        this._renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this._renderer.material.color = this.InLandingColor;
            this.onGrounded.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this._renderer.material.color = this.InNotLandingColor;
            this.onLeaveFromGround.Invoke();
        }
    }
}
