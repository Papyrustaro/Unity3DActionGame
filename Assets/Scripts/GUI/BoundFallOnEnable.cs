using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class BoundFallOnEnable : MonoBehaviour
{
    [SerializeField] private float fallLength = 100f;
    [SerializeField] private float animationTime = 3f;
    private RectTransform _rectTransform;

    private void Awake()
    {
        this._rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        this._rectTransform.DOLocalMoveY(-1 * this.fallLength, this.animationTime)
            .SetLink(this.gameObject)
            .SetRelative(true)
            .SetEase(Ease.OutBounce);
    }
}
