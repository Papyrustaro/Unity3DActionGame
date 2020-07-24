using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainInputUIManager : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text explainInputText;
    [SerializeField] private Button changeTextButton;
    [SerializeField] private Text changeTextButtonText;
    private bool isShowingKeybordInput = true;

    public void ChangeInputText()
    {
        if (this.isShowingKeybordInput)
        {
            this.titleText.text = "操作方法(Joy-Con)";
            this.explainInputText.text = "移動: Lスティック\n通常ジャンプ: A\nスピンジャンプ: X\nバックフリップ: Y\nヒップドロップ: B" +
                "\nカメラ回転: Rスティック\n正面が見えるようにカメラ移動: L1\n決定: A";
            this.changeTextButtonText.text = "キーボードの操作を確認";
        }
        else
        {
            this.titleText.text = "操作方法(キーボード)";
            this.explainInputText.text = "移動: WA(前後),SD(左右)\n通常ジャンプ: J/Space\nスピンジャンプ: L\nバックフリップ: I\nヒップドロップ: U\n" +
                "カメラ回転: 方向キー/78(上下),90(左右)\n正面が見えるようにカメラ移動: K\n決定: Enter/Space\n";
            this.changeTextButtonText.text = "Joy-Conの操作を確認";
        }
        this.isShowingKeybordInput = !this.isShowingKeybordInput;
    }
}
