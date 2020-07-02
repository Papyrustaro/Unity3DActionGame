using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;

public class UniTaskSample : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        var token = this.GetCancellationTokenOnDestroy();
        DoButtonAsync(token).Forget(); 
    }

    private async UniTask WaitAsync()
    {
        Debug.Log("a");
        await UniTask.Delay(1000);
        Debug.Log("i");
        await UniTask.DelayFrame(60);
        Debug.Log("u");
    }

    private async UniTaskVoid DoButtonAsync(CancellationToken cancellationToken)
    {
        var asyncEventHandler = button.onClick.GetAsyncEventHandler(cancellationToken);

        await asyncEventHandler.OnInvokeAsync();

        Debug.Log("1回押された");

        await asyncEventHandler.OnInvokeAsync();

        Debug.Log("2回押された");
    }

    private async UniTask<string> Work()
    {
        return await Task.Run(() => "Hello");
    }
}
