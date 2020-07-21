using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeepMoveUntilDestination : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Vector3> path;
    [SerializeField] private Color colorInMoving;
    private Sequence _sequence;
    private Color colorInNotMoving;
    private Renderer _renderer;

    public bool CanMove { get; set; } = false;

    private void Awake()
    {
        this._renderer = GetComponent<Renderer>();
        this.colorInNotMoving = this._renderer.material.color;
    }

    private void Start()
    {
        this._sequence = DOTween.Sequence()
            .SetRelative()
            .SetLink(this.gameObject)
            .OnPlay(() => this._renderer.material.color = this.colorInMoving)
            .OnPause(() => { if (StageTimeManager.Instance.IsStageMoving) this._renderer.material.color = this.colorInNotMoving; })
            .OnComplete(() => this._renderer.material.color = this.colorInNotMoving);
        foreach(Vector3 v in path)
        {
            this._sequence.Append(this.transform.DOMove(v, v.magnitude / this.moveSpeed));
        }
        this._sequence.Pause();
    }

    private void Update()
    {
        if(this._sequence.IsPlaying() && !StageTimeManager.Instance.IsStageMoving)
        {
            this._sequence.Pause();
        }
        if(!this._sequence.IsPlaying() && StageTimeManager.Instance.IsStageMoving && this.CanMove)
        {
            this._sequence.Play();
        }
    }
}
