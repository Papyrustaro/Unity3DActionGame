using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class UniRxSampleTimerCounter : MonoBehaviour
{
    [SerializeField] private int timeLeft = 3;

    private Subject<int> timerSubject = new Subject<int>();

    public IObservable<int> OnTimeChanged
    {
        get { return this.timerSubject; }
    }

    private void Start()
    {
        StartCoroutine(TimerCoroutine());

        this.timerSubject.Subscribe(x => Debug.Log(x));
    }

    IEnumerator TimerCoroutine()
    {
        yield return null;
        int time = this.timeLeft;
        while(time >= 0)
        {
            timerSubject.OnNext(time--);
            yield return new WaitForSeconds(1);
        }
        this.timerSubject.OnCompleted();
    }
}
