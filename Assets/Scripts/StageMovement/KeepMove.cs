using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class KeepMove : MonoBehaviour
{
    [SerializeField] private Vector3 horizontalDirection;
    [SerializeField] private float verticalLength;
    [SerializeField] private float moveSpeed = 1f;
    private Sequence sequence;
    private bool isMoving = true;

    private void Start()
    {
        MoveSquare(this.horizontalDirection, this.verticalLength, this.moveSpeed);
    }

    private void Update()
    {
        if (this.isMoving && !StageTimeManager.Instance.IsStageMoving)
        {
            this.isMoving = false;
            this.sequence.Pause();
        }
        if(!this.isMoving && StageTimeManager.Instance.IsStageMoving)
        {
            this.isMoving = true;
            this.sequence.Play();
        }
    }

    /// <summary>
    /// 特定の垂直平面において(反)時計回りに長方形移動し続ける
    /// </summary>
    /// <param name="horizontalDirection">水平方向移動量</param>
    /// <param name="verticalLength">垂直方向移動量(正のとき初期位置から上がって下がる)</param>
    /// <param name="moveSpeed">移動する速さ</param>
    private void MoveSquare(Vector3 horizontalDirection, float verticalLength, float moveSpeed)
    {
        if (horizontalDirection.y != 0) throw new System.Exception();

        float horizontalLength = horizontalDirection.magnitude;
        float oneRoutineTime = (2 * horizontalLength + 2 * verticalLength) / moveSpeed;
        float horizontalTime = oneRoutineTime * (horizontalLength / (2 * horizontalLength + 2 * verticalLength));
        float verticalTime = oneRoutineTime * (verticalLength / (2 * horizontalLength + 2 * verticalLength));

        this.sequence = DOTween.Sequence();
        this.sequence
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.up * verticalLength, verticalTime))
            .Append(this.transform.DOBlendableLocalMoveBy(horizontalDirection, horizontalTime))
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.down * verticalLength, verticalTime))
            .Append(this.transform.DOBlendableLocalMoveBy(horizontalDirection * -1, horizontalTime))
            .SetRelative()
            .SetLoops(-1)
            .SetLink(this.gameObject)
            .Play();
    }
}
