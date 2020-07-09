using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UniRx;
using NaughtyAttributes;

/// <summary>
/// 決められた軌道で移動するリフト
/// </summary>
public class LiftMovement : MonoBehaviour
{
    [SerializeField] private E_GroundedState moveState;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector3[] path;
    [SerializeField] private Color onMoveColor;
    [SerializeField] private Color orbitLineColor;
    [SerializeField] private Material orbitLineMaterial;
    private E_GroundedState currentGroundedState = E_GroundedState.NotGrounded;
    private Sequence sequence;
    private Color defaultColor;
    private Renderer _renderer;

    private void Awake()
    {
        this._renderer = GetComponent<Renderer>();
        this.defaultColor = this._renderer.material.color;
        /*this
            .ObserveEveryValueChanged(x => x.currentGroundedState)
            .ThrottleFrame(5)
            .Subscribe(x => this.currentGroundedState = x);*/
    }

    private void Start()
    {
        this.sequence = DOTween.Sequence()
            .SetRelative()
            .SetLink(this.gameObject)
            .SetLoops(-1);
        if(this.moveState != E_GroundedState.Always)
        {
            this.sequence
                .OnPlay(() => this._renderer.material.color = this.onMoveColor)
                .OnPause(() => { if (StageTimeManager.Instance.IsStageMoving) this._renderer.material.color = this.defaultColor; });
        }
        foreach(Vector3 p in this.path.Concat(this.path.Reverse().Select(v => v * -1f)))
        {
            this.sequence.Append(this.transform.DOMove(p, p.magnitude / this.moveSpeed));
        }
        this.sequence.Play();
    }

    private void Update()
    {
        if(this.sequence.IsPlaying())
        {
            if (!StageTimeManager.Instance.IsStageMoving || (this.moveState == E_GroundedState.Grounded && this.currentGroundedState == E_GroundedState.NotGrounded) ||
            (this.moveState == E_GroundedState.NotGrounded && this.currentGroundedState == E_GroundedState.Grounded))
            {
                this.sequence.Pause();
            }
        }
        else
        {
            if (StageTimeManager.Instance.IsStageMoving && ((this.moveState == E_GroundedState.Grounded && this.currentGroundedState == E_GroundedState.Grounded) ||
            (this.moveState == E_GroundedState.NotGrounded && this.currentGroundedState == E_GroundedState.NotGrounded) || this.moveState == E_GroundedState.Always))
            {
                this.sequence.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMoveGroundCheck"))
        {
            this.currentGroundedState = E_GroundedState.Grounded;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerMoveGroundCheck"))
        {
            this.currentGroundedState = E_GroundedState.NotGrounded;
        }
    }

    /// <summary>
    /// Inspectorからリフトの軌道上の線を作成
    /// </summary>
    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void GenericLineOfOrbit()
    {
        List<Vector3> paths = new List<Vector3>();
        paths.Add(this.transform.position);
        for(int i = 0; i < this.path.Length; i++)
        {
            paths.Add(this.path[i] + paths[i]);
        }
        LineRenderer lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = this.orbitLineMaterial;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = this.orbitLineColor;
        lineRenderer.endColor = this.orbitLineColor;
        lineRenderer.positionCount = paths.Count;
        lineRenderer.SetPositions(paths.ToArray());
        
    }
}

/// <summary>
/// 接地判定※記述場所等は要修正
/// </summary>
public enum E_GroundedState
{
    Always,
    Grounded,
    NotGrounded,
    Other
}
