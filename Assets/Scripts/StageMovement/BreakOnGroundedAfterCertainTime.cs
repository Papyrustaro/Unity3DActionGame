using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーが着地して一定時間後に崩れる床
/// </summary>
public class BreakOnGroundedAfterCertainTime : MonoBehaviour
{
    [SerializeField] private float breakTime = 3f;
    [SerializeField] private UnityEvent onBreak;
    private bool startCount = false;
    private float countTime = 0f;
    private void Update()
    {
        if (this.startCount && StageTimeManager.Instance.IsStageMoving)
        {
            this.countTime += Time.deltaTime;
            if(this.countTime > this.breakTime)
            {
                this.onBreak.Invoke();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGroundCheck"))
        {
            this.startCount = true;
        }
    }
}
