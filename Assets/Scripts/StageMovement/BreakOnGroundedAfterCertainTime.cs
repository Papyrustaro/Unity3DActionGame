using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーが着地して一定時間後に崩れる床(消えた後復活しない: 要修正)
/// </summary>
public class BreakOnGroundedAfterCertainTime : MonoBehaviour
{
    [SerializeField] private float breakTime = 1f;
    [SerializeField] private float respawnTime = 3f;
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
                this.countTime = 0f;
                StartCoroutine(DelayRespawn(this.respawnTime));
                this.gameObject.SetActive(false);
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

    public IEnumerator DelayRespawn(float waitTime)
    {
        if (!StageTimeManager.Instance.IsStageMoving) yield return null;
        yield return new WaitForSeconds(waitTime);
        this.gameObject.SetActive(true);
    }
}
