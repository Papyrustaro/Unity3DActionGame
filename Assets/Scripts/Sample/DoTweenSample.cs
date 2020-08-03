using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class DoTweenSample : MonoBehaviour
{
    private Tween currentPlayTween;
    private UnityEvent onDestroy = new UnityEvent();
    //private Sequence currentSequence;

    private void Start()
    {
        //MoveSquareSample(3f, 2f, 2f);
        //MoveSquareSample(3f, 2f, 2f);
        //MoveSquareSample(new Vector3(3f, 0f, 3f), 2f, 2f);
        MoveSquareSample(new Vector3(20f, 0f, 0f), 0f, 10f);
        //RotationSample(1f, 5f, new Vector3(2, 0, 1));
        //RotationSample();
        //Sampleple();
        Invoke("DestroyMe", 30f);
    }

    private void Update()
    {
        //this.transform.RotateAround(this.transform.position, this.transform.up, 10f);
        //this.transform.RotateAround(this.transform.position, Vector3.right, 1f);
    }
    private void PlayMoveTween()
    {
        float moveDuration = 2f;
        Vector3 endPosition = new Vector3(10f, 10f, 0f);
        this.currentPlayTween = this.transform.DOMove(endPosition, moveDuration);
        this.currentPlayTween.SetRelative();
        this.currentPlayTween.SetEase(Ease.Linear);
        this.currentPlayTween.Play();
    }

    /// <summary>
    /// きりもみ回転
    /// </summary>
    /// <param name="durationOfRotationUpAxis">transform.upを軸とした1回転の時間</param>
    /// <param name="durationOfRotationHorizontalAxis">horizontalAxisを軸とした1回転の時間</param>
    /// <param name="horizontalAxis">水平方向の回転軸。yは0</param>
    private void RotationSample(float durationOfRotationUpAxis, float durationOfRotationHorizontalAxis, Vector3 horizontalAxis)
    {
        if (horizontalAxis.y != 0f) throw new System.Exception();
        horizontalAxis = horizontalAxis.normalized;
        Sequence sequence = DOTween.Sequence();
        sequence
            .SetLoops(-1)
            .OnUpdate(() => 
            {
                this.transform.RotateAround(this.transform.position, horizontalAxis, 360f * Time.deltaTime / durationOfRotationHorizontalAxis);
                this.transform.RotateAround(this.transform.position, this.transform.up, 360f * Time.deltaTime / durationOfRotationUpAxis); 
            })
            .Play();
        this.onDestroy.AddListener(() => sequence.Kill());
    }

    private void Sampleple()
    {
        Quaternion currentRotation = this.transform.rotation;
        Debug.Log(Time.time);
        Sequence sequence = DOTween.Sequence()
            //.SetRelative()
            .Append(this.transform.DORotateQuaternion(currentRotation, 0f))
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.right * 10f, 1f))
            .Append(this.transform.DOBlendableRotateBy(Vector3.right * 90f, 1f, RotateMode.WorldAxisAdd))
            .OnStepComplete(() => { currentRotation = this.transform.rotation; Debug.Log(Time.time); })
            .SetLoops(-1)
            .Play();
        this.onDestroy.AddListener(() => sequence.Kill());
    }

    private void MoveSample()
    {
        this.transform.DOBlendableMoveBy(new Vector3(10, 0, 0), 10);
        this.transform.DOBlendableMoveBy(new Vector3(0, 10, 0), 10);
    }

    private void MoveEaseSample()
    {
        this.transform.DOBlendableLocalMoveBy(new Vector3(10, 0, 0), 10f)
            .SetDelay(1f)
            .SetRelative()
            .SetEase(Ease.InBounce)
            .SetLoops(2);
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(this.transform.DOMoveX(6f, 3f))
            .Play();
        this.onDestroy.AddListener(() => sequence.Kill());

    }


    private void MoveSquareSample(float horizontalLength, float verticalLength, float oneRoutineTime)
    {
        float horizontalTime = oneRoutineTime * (horizontalLength / (2 * horizontalLength + 2 * verticalLength));
        float verticalTime = oneRoutineTime * (verticalLength / (2 * horizontalLength + 2 * verticalLength));

        Sequence sequence = DOTween.Sequence();
        sequence
            /*OnUpdate(() =>
            {
                if (this == null) sequence.Kill();
            })*/
            .OnStepComplete(() =>
            {
                Debug.Log("A");
            })
            .Append(this.transform.DOMoveX(horizontalLength, horizontalTime))
            .Append(this.transform.DOMoveY(-verticalLength, verticalTime))
            .Append(this.transform.DOMoveX(-horizontalLength, horizontalTime))
            .Append(this.transform.DOMoveY(verticalLength, verticalTime))
            .SetRelative()
            .SetLoops(-1)
            .Play();
        this.onDestroy.AddListener(()=>sequence.Kill());
    }

    /// <summary>
    /// 特定の垂直平面において(反)時計回りに長方形移動し続ける
    /// </summary>
    /// <param name="horizontalDirection">水平方向移動量</param>
    /// <param name="verticalLength">垂直方向移動量(正のとき初期位置から上がって下がる)</param>
    /// <param name="oneRoutineTime">長方形を一周する時間</param>
    private void MoveSquareSample(Vector3 horizontalDirection, float verticalLength, float oneRoutineTime)
    {
        if (horizontalDirection.y != 0) throw new System.Exception();
        float horizontalLength = horizontalDirection.magnitude;
        float horizontalTime = oneRoutineTime * (horizontalLength / (2 * horizontalLength + 2 * verticalLength));
        float verticalTime = oneRoutineTime * (verticalLength / (2 * horizontalLength + 2 * verticalLength));

        Sequence sequence = DOTween.Sequence();
        sequence
            .OnStepComplete(() =>
            {
                Debug.Log("1ループ終了");
            })
            .AppendCallback(() => Debug.Log("ループ開始"))
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.up * verticalLength, verticalTime))
            .Append(this.transform.DOBlendableLocalMoveBy(horizontalDirection, horizontalTime))
            .Append(this.transform.DOBlendableLocalMoveBy(Vector3.down * verticalLength, verticalTime))
            .Append(this.transform.DOBlendableLocalMoveBy(horizontalDirection * -1, horizontalTime))
            .SetRelative()
            .SetLoops(-1)
            .SetLink(this.gameObject)
            .Play();
        //this.onDestroy.AddListener(() => sequence.Kill());
    }


    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        this.onDestroy.Invoke();
    }
}