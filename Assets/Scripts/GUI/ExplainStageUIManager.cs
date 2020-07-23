using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マニュアルのステージギミック説明のUI操作
/// </summary>
public class ExplainStageUIManager : MonoBehaviour
{
    [SerializeField] private Text stageNameText;
    [SerializeField] private Text explainStageText;
    [SerializeField] private Image explainStageImage;

    [SerializeField] private List<Sprite> explainStageSprites = new List<Sprite>();

    private readonly string[] stageName =
    {
        "移動リフト", "トランポリン", "加速床", "スイッチ", "壊せるブロック", "ワープ", "ゴール"
    };

    private readonly string[] explainStage =
    {
        "乗ることができるリフト\n進んでいる間は赤くなる\n・乗っている間移動\n・乗っていない間移動\n・一度乗ったら移動\nの3種類がある",
        "乗ると跳ねるトランポリン\n　状態に合わせて、ジャンプボタンまたはスピンジャンプボタン長押しで高く飛ぶことができる",
        "乗っている間速く走ることができる\n速すぎてそのまま落ちないように",
        "赤青切り替わるスイッチと\nそれに対応して現れるステージ\nスイッチはジャンプで叩くほか、\nヒップドロップでも切り替わる\n一定時間で切り替わるものも",
        "下から叩くかヒップドロップで壊すことができるブロック",
        "触れると特定地点にワープする",
        "触れるとステージクリアとなる"
    };

    private int currentIndex = 0;
    private int stageTypeCount = 0;

    private void Awake()
    {
        this.stageTypeCount = this.stageName.Length;
        this.stageNameText.text = this.stageName[this.currentIndex];
        this.explainStageText.text = this.explainStage[this.currentIndex];
        this.explainStageImage.sprite = this.explainStageSprites[this.currentIndex];
    }

    public void GoNextStageExplain()
    {
        this.currentIndex++;
        if (this.currentIndex > this.stageTypeCount - 1) this.currentIndex = 0;
        this.stageNameText.text = this.stageName[this.currentIndex];
        this.explainStageText.text = this.explainStage[this.currentIndex];
        this.explainStageImage.sprite = this.explainStageSprites[this.currentIndex];
    }

    public void GoBackStageExplain()
    {
        this.currentIndex--;
        if (this.currentIndex < 0) this.currentIndex = this.stageName.Length - 1;
        this.stageNameText.text = this.stageName[this.currentIndex];
        this.explainStageText.text = this.explainStage[this.currentIndex];
        this.explainStageImage.sprite = this.explainStageSprites[this.currentIndex];
    }


}

/*----------------
 * 0.リフト
 * 1.トランポリン
 * 2.加速床
 * 3.スイッチ
 * 4.消える床
 * 5.壊せるブロック
----------------*/
