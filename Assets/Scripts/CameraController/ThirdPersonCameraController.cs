using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの背後を基本とし、カメラを移動させる
/// </summary>
public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 defaultPosition = new Vector3(0f, 2f, -5f);
    [SerializeField] private Vector3 onePersonPosition = new Vector3(0f, 1f, 0.5f);

    [SerializeField] private Vector3 overheadPosition = new Vector3(0f, 15f, -1f);
    [SerializeField] private Vector3 overheadRotation = new Vector3(85f, 0f, 0f);

    private float rotationByMouseForce = 200f;
    private Vector3 targetPositionBeforeFrame;

    public static ThirdPersonCameraController Instance { get; private set; }

    public bool IsMoving { get; set; } = true;

    public E_CameraViewType CameraViewType { private get; set; } = E_CameraViewType.Default;

    public Transform TargetPlayerCenterTransform { get; set; } = null;



    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }
    }

    private void LateUpdate()
    {
        if(this.TargetPlayerCenterTransform == null)
        {
            if (PlayerManager.Instance == null) return;
            this.TargetPlayerCenterTransform = PlayerManager.Instance.CenterTransform;
            this.targetPositionBeforeFrame = this.TargetPlayerCenterTransform.position;
            this.IsMoving = true;
        }
        if (!this.IsMoving) return;

        this.transform.position += this.TargetPlayerCenterTransform.position - this.targetPositionBeforeFrame;
        this.targetPositionBeforeFrame = this.TargetPlayerCenterTransform.position;

        if (Input.GetMouseButton(1))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            this.transform.RotateAround(this.targetPositionBeforeFrame, Vector3.up, mouseInputX * Time.deltaTime * this.rotationByMouseForce);
            if(this.CameraViewType != E_CameraViewType.Overhead) this.transform.RotateAround(this.targetPositionBeforeFrame, this.transform.right, mouseInputY * Time.deltaTime * this.rotationByMouseForce);
        }

        if (Input.GetButtonDown("CameraMoveToPlayerBehind"))
        {
            WatchFromPlayerBack();
        }
        if (Input.GetButtonDown("DefaultCamera"))
        {
            WatchDefault();
        }
        if (Input.GetButtonDown("OnePersonCamera"))
        {
            WatchOnePerson();
        }
        if (Input.GetButtonDown("OverheadCamera"))
        {
            WatchFromOverHead();
        }
    }

    /// <summary>
    /// プレイヤーの正面を向く(カメラをプレイヤーの背中側に移動)。ゆっくり回転させたい気もする
    /// 一人称視点では、マウスでのみ向いている方向が変わるべきではある。
    /// </summary>
    private void WatchFromPlayerBack()
    {
        if(this.CameraViewType == E_CameraViewType.OnePerson)
        {
            this.transform.RotateAround(this.TargetPlayerCenterTransform.position, Vector3.up, this.transform.rotation.eulerAngles.y - this.TargetPlayerCenterTransform.rotation.eulerAngles.y);
        }
        else
        {
            this.transform.RotateAround(this.TargetPlayerCenterTransform.position, Vector3.up, this.TargetPlayerCenterTransform.rotation.eulerAngles.y - this.transform.rotation.eulerAngles.y);
        }

        //上下も戻すとき
        if(this.CameraViewType != E_CameraViewType.Overhead)
        {
            this.transform.RotateAround(this.TargetPlayerCenterTransform.position, this.transform.right, this.TargetPlayerCenterTransform.rotation.eulerAngles.x - this.transform.rotation.eulerAngles.x);
        }
    }

    /// <summary>
    /// プレイヤーを真上から見る(カメラをプレイヤーの真上に移動)
    /// </summary>
    private void WatchFromOverHead()
    {
        this.CameraViewType = E_CameraViewType.Overhead;
        this.transform.position = this.TargetPlayerCenterTransform.position + this.overheadPosition;
        this.transform.rotation = Quaternion.Euler(this.overheadRotation);
    }

    /// <summary>
    /// 一人称視点
    /// </summary>
    private void WatchOnePerson()
    {
        this.CameraViewType = E_CameraViewType.OnePerson;
        this.transform.position = this.TargetPlayerCenterTransform.position + this.onePersonPosition;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    /// <summary>
    /// defaultの三人称視点
    /// </summary>
    private void WatchDefault()
    {
        this.CameraViewType = E_CameraViewType.Default;
        this.transform.position = this.TargetPlayerCenterTransform.position + this.defaultPosition;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    public enum E_CameraViewType
    {
        Default,
        Overhead,
        OnePerson,
    }
}
