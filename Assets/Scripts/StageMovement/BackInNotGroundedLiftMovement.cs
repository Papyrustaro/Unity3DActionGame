using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


/// <summary>
/// 乗っている間だけ移動、降りると戻っていくリフト。
/// </summary>
public class BackInNotGroundedLiftMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float backSpeed = 0.5f;
    [SerializeField] private Vector3[] path;
    private Sequence sequence;
    private bool isRewarding = false;
    private float moveTime = 0f;
    private float currentMoveTime = 0f;


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
            .OnUpdate(() =>
            {
                if (this.isRewarding) this.currentMoveTime -= Time.deltaTime * (this.backSpeed / this.moveSpeed);
                else this.currentMoveTime += Time.deltaTime;

                if (this.currentMoveTime > this.moveTime - 0.1f) { this.currentMoveTime = this.moveTime - 0.1f; this.sequence.Pause(); }
            });

        foreach (Vector3 p in this.path)
        {
            this.sequence.Append(this.transform.DOMove(p, p.magnitude / this.moveSpeed));
            this.moveTime += p.magnitude / this.moveSpeed;
        }
        this.sequence.Pause();
    }

    private void Update()
    {
        if (this.sequence.IsPlaying() && !StageTimeManager.Instance.IsStageMoving)
        {
            this.sequence.Pause();
        }
        else if(!this.sequence.IsPlaying() && StageTimeManager.Instance.IsStageMoving && this.currentMoveTime > 0f)
        {
            this.sequence.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this.sequence.timeScale = 1f;
            this.sequence.PlayForward();
            this.isRewarding = false;
            this._renderer.material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this.sequence.timeScale = this.backSpeed / this.moveSpeed;
            this.sequence.SmoothRewind();
            this.sequence.Play();
            this.isRewarding = true;
            this._renderer.material.color = Color.white;
        }
    }

    /// <summary>
    /// Inspectorからリフトの軌道上の線を作成
    /// </summary>
    [ContextMenu("GenerateLineOfOrbit")]
    public void GenericLineOfOrbit()
    {
        List<Vector3> paths = new List<Vector3>();
        paths.Add(this.transform.position);
        for (int i = 0; i < this.path.Length; i++)
        {
            paths.Add(this.path[i] + paths[i]);
        }
        LineRenderer lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = paths.Count;
        lineRenderer.SetPositions(paths.ToArray());
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
}
