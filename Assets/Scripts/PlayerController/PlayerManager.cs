using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private MonobitEngine.MonobitView _monobitView;

    [field: SerializeField]
    [field: RenameField("centerTransform")]
    public Transform CenterTransform { get; private set; }
    public static PlayerManager Instance { get; private set; }
    
    private void Awake()
    {
        this._monobitView = GetComponent<MonobitEngine.MonobitView>();
        if (this._monobitView != null && !this._monobitView.isMine) return;

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }
    }

    
}
