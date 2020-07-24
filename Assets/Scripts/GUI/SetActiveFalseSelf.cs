using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalseSelf : MonoBehaviour
{
    public void SetFalseSelf()
    {
        this.gameObject.SetActive(false);
    }
}
