using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UniRxSamplePlayerMover : MonoBehaviour
{
    [SerializeField] private UniRxSampleTimerCounter _timeCounter;

    private float _moveSpeed = 10.0f;

    private void Start()
    {
        this._timeCounter.OnTimeChanged
            .Where(x => x == 0)
            .Subscribe(_ =>
            {
                this.transform.position = Vector3.zero;
            }).AddTo(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
        }

        if(this.transform.position.x > 10)
        {
            Debug.Log("画面外");
            Destroy(this.gameObject);
        }
    }
}
