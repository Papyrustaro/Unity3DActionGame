using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの背後を基本とし、カメラを移動させる
/// </summary>
public class ThirdPersonCameraController : MonoBehaviour
{
    //[SerializeField] private Vector3 defaultPosition = new Vector3(0f, 2f, -5f);
    //[SerializeField] private Vector3 onePersonPosition = new Vector3(0f, 1f, 0.5f);

    //[SerializeField] private Vector3 overheadPosition = new Vector3(0f, 15f, -1f);
    //[SerializeField] private Vector3 overheadRotation = new Vector3(85f, 0f, 0f);

    private float rotationByMouseForce = 200f;
    private Vector3 targetPositionBeforeFrame;
    private float defaultCameraRotationX;
    private Quaternion initRotation;

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
        this.initRotation = this.transform.rotation;
        this.defaultCameraRotationX = this.transform.rotation.eulerAngles.x;
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

        float cameraAxisY = Input.GetAxis("RotateCameraAxisY");
        float cameraAxisHorizontal = Input.GetAxis("RotateCameraAxisHorizontal");
        float cameraRotationAxisHorizontal = this.transform.rotation.eulerAngles.x;

        if (StaticData.invertCameraRotationAxisY) cameraAxisY *= -1f;
        if (StaticData.invertCameraRotationHorizontal) cameraAxisHorizontal *= -1f;
        if(cameraAxisY != 0) this.transform.RotateAround(this.targetPositionBeforeFrame, Vector3.up, cameraAxisY * Time.deltaTime * this.rotationByMouseForce);
        if((((355f < cameraRotationAxisHorizontal && cameraRotationAxisHorizontal <= 360f) || (cameraRotationAxisHorizontal < 75f)) && cameraAxisHorizontal != 0) ||
            (340f < cameraRotationAxisHorizontal && cameraRotationAxisHorizontal <= 355f && cameraAxisHorizontal > 0) ||
            (75f < cameraRotationAxisHorizontal && cameraRotationAxisHorizontal < 90f && cameraAxisHorizontal < 0))
            this.transform.RotateAround(this.targetPositionBeforeFrame, this.transform.right, cameraAxisHorizontal * Time.deltaTime * this.rotationByMouseForce);
        /*if (Input.GetMouseButton(1))
        {
            //float mouseInputX = Input.GetAxis("Mouse X");
            //float mouseInputY = Input.GetAxis("Mouse Y");
            //this.transform.RotateAround(this.targetPositionBeforeFrame, Vector3.up, mouseInputX * Time.deltaTime * this.rotationByMouseForce);
            
            //if(this.CameraViewType != E_CameraViewType.Overhead) this.transform.RotateAround(this.targetPositionBeforeFrame, this.transform.right, mouseInputY * Time.deltaTime * this.rotationByMouseForce);
        }*/

        if (Input.GetButtonDown("RotateCamera90LeftAxisY"))
        {
            this.Rotate90LeftAxisY();
        }
        if (Input.GetButtonDown("RotateCamera90RightAxisY"))
        {
            this.Rotate90RightAxisY();
        }
        if (Input.GetButtonDown("InitCameraRotation"))
        {
            this.InitCameraRotation();
        }
        /*if (Input.GetButtonDown("CameraMoveToPlayerBehind"))
        {
            WatchFromPlayerBack();
        }*/
        /*if (Input.GetButtonDown("DefaultCamera"))
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
        }*/
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
            this.transform.RotateAround(this.TargetPlayerCenterTransform.position, this.transform.right, this.defaultCameraRotationX - this.transform.rotation.eulerAngles.x);
        }
    }

    /// <summary>
    /// -90,0,90,180の°に沿う用に右回転
    /// </summary>
    private void Rotate90RightAxisY()
    {
        int angleLevel = (int)(this.transform.rotation.eulerAngles.y / 90) + 1;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, angleLevel * 90f, this.transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// -90,0,90,180の°に沿う用に左回転
    /// </summary>
    private void Rotate90LeftAxisY()
    {
        int angleLevel = (int)(this.transform.rotation.eulerAngles.y / 90);
        if (this.transform.rotation.eulerAngles.y % 90 < 1) angleLevel--;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, angleLevel * 90f, this.transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// ステージ開始時のカメラ角度に初期化
    /// </summary>
    private void InitCameraRotation()
    {
        this.transform.rotation = this.initRotation;
    }

    /// <summary>
    /// プレイヤーを真上から見る(カメラをプレイヤーの真上に移動)
    /// </summary>
    /*private void WatchFromOverHead()
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
    }*/

    public enum E_CameraViewType
    {
        Default,
        Overhead,
        OnePerson,
    }
}
