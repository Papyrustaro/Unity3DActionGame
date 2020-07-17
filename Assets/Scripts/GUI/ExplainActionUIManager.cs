using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マニュアルのアクション説明のUI操作
/// </summary>
public class ExplainActionUIManager : MonoBehaviour
{
    [SerializeField] private Text actionNameText;
    [SerializeField] private Text explainActionText;
    [SerializeField] private Image explainActionImage;

    [SerializeField] private List<Sprite> explainActionSprites = new List<Sprite>();

    private readonly string[] actionName =
    {
        "通常ジャンプ", "スピンジャンプ", "バックフリップ", "ヒップドロップ", "壁張り付き", "壁キック"
    };

    private readonly string[] explainAction =
    {
        "上方向にジャンプする\nジャンプの高さはジャンプボタンの入力時間で3段階に分かれる",
        "くるくる回転しながら飛ぶ\n通常ジャンプよりも落下スピードが遅く、横移動はしやすい。最高点は少し低い",
        "後ろ方向に回転しながら飛ぶ\nかなり高く飛べるが、空中制御が効かない",
        "空中にいるときにいつでもできる\n1回転したあと、真下に凄い速さで落下する\n着地するまで操作できないのでご用心",
        "壁方向を向きながら空中でぶつかると、張り付くことができる\n張り付き中はゆっくり落下する\n壁に沿って横移動したり、そのまま壁から離れることもできる",
        "壁に張り付いている状態でジャンプすると、壁を蹴ることができる\n張り付き状態時の横移動の勢いは残る"
    };

    private int currentIndex = 0;
    private int actionTypeCount = 0;

    private void Awake()
    {
        this.actionTypeCount = this.actionName.Length;
        this.actionNameText.text = this.actionName[this.currentIndex];
        this.explainActionText.text = this.explainAction[this.currentIndex];
        this.explainActionImage.sprite = this.explainActionSprites[this.currentIndex];
    }

    public void GoNextActionExplain()
    {
        this.currentIndex++;
        if (this.currentIndex > this.actionTypeCount - 1) this.currentIndex = 0;
        this.actionNameText.text = this.actionName[this.currentIndex];
        this.explainActionText.text = this.explainAction[this.currentIndex];
        this.explainActionImage.sprite = this.explainActionSprites[this.currentIndex];
    }

    public void GoBackActionExplain()
    {
        this.currentIndex--;
        if (this.currentIndex < 0) this.currentIndex = this.actionName.Length - 1;
        this.actionNameText.text = this.actionName[this.currentIndex];
        this.explainActionText.text = this.explainAction[this.currentIndex];
        this.explainActionImage.sprite = this.explainActionSprites[this.currentIndex];
    }

    
}

/*----------------
 * 0.ジャンプ
 * 1.スピンジャンプ
 * 2.バックフリップ
 * 3.ヒップドロップ
 * 4.壁張り付き
 * 5.壁キック
----------------*/
