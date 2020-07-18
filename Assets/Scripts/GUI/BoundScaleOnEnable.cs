using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoundScaleOnEnable : MonoBehaviour
{
    [SerializeField] private float scaleRate = 1.3f;
    [SerializeField] private float animationTime = 1f;
    private RectTransform _rectTransform;

    private void Awake()
    {
        this._rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        this._rectTransform.DOScale(this.scaleRate, this.animationTime)
            .SetLink(this.gameObject)
            .SetRelative(true)
            .SetEase(Ease.OutBounce);
    }
}
