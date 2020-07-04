using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Animations;
using DG.Tweening;
using System.Linq;

/// <summary>
/// 決められた軌道で移動するリフト
/// </summary>
public class LiftMovement : MonoBehaviour
{
    [SerializeField] private E_GroundedState moveState;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector3[] path;
    private E_GroundedState currentGroundedState = E_GroundedState.NotGrounded;
    private Sequence sequence;

    private Renderer _renderer;

    private void Awake()
    {
        this._renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        this.sequence = DOTween.Sequence()
            .SetRelative()
            .SetLink(this.gameObject)
            .SetLoops(-1);
        foreach(Vector3 p in this.path.Concat(this.path.Reverse().Select(v => v * -1f)))
        {
            this.sequence.Append(this.transform.DOMove(p, p.magnitude / this.moveSpeed));
        }
        this.sequence.Play();
    }

    private void Update()
    {
        if (this.sequence.IsPlaying())
        {
            if (!StageTimeManager.Instance.StageAbleMove || (this.moveState == E_GroundedState.Grounded && this.currentGroundedState == E_GroundedState.NotGrounded) ||
            (this.moveState == E_GroundedState.NotGrounded && this.currentGroundedState == E_GroundedState.Grounded))
            {
                this.sequence.Pause();
            }
        }
        else
        {
            if (StageTimeManager.Instance.StageAbleMove && ((this.moveState == E_GroundedState.Grounded && this.currentGroundedState == E_GroundedState.Grounded) ||
            (this.moveState == E_GroundedState.NotGrounded && this.moveState == E_GroundedState.NotGrounded) || this.moveState == E_GroundedState.All))
            {
                this.sequence.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this.currentGroundedState = E_GroundedState.Grounded;
            this._renderer.material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this.currentGroundedState = E_GroundedState.NotGrounded;
            this._renderer.material.color = Color.white;
        }
    }
}

/// <summary>
/// 接地判定※記述場所等は要修正
/// </summary>
public enum E_GroundedState
{
    All,
    Grounded,
    NotGrounded,
    Other
}
