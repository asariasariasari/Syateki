using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;

public class Alignment : MonoBehaviour
{

    //可動制限のための変数
    private Vector3 max;
    private Vector3 min;
    private Action playerApdate;

    //プレイヤーと関連づけるための変数
    private int number;

    private RectTransform reTf;

    public int Number { set { number = value; } }

    public void Init()
    {
        reTf = GetComponent<RectTransform>();

        //子としてインスタンスするために親オブジェクトを参照しています
        var alignments = GameObject.Find("alignments");
        //ここでcanvas上の親オブジェクトの子にしています
        reTf.SetParent(alignments.GetComponent<RectTransform>(), false);

        if (GameManager.Instance.JoyconMode) playerApdate += () => playerApdate();
        else playerApdate += () => MouseMove();
        playerApdate += () => Look();

        //照準の動きに制限をかけるために色々参照しています
        //ここで参照しているのはこのオブジェクトの親であるalignmentsではなくそれ上にあるUI(Canvas)を参照しています
        var alignmentParentRectTf = transform.root.gameObject.GetComponent<RectTransform>();
        var alignmentParentScale = alignmentParentRectTf.localScale.x;
        var alignmentParentPos = alignmentParentRectTf.position;
        max = new Vector3(400f, 225f, 0f) * alignmentParentScale + alignmentParentPos;
        min = new Vector3(-400f, -225f, 0f) * alignmentParentScale + alignmentParentPos;
    }

    // Update is called once per frame
    void Update()
    {
        playerApdate();
    }

    //マウスに応じて動かすメソッドです。デバッグ用
    private void MouseMove()
    {
        //ベクトル作成
        var position = Input.mousePosition;
        position.z = Camera.main.transform.position.z * -1f;
        var cameraWordPos = Camera.main.ScreenToWorldPoint(position);

        transform.position = cameraWordPos;

        AlignmenPosLimit();
    }

    //ジョイコンで動かす用のメソッド
    /*private void JoyconMove()
    {
        //ベクトル作成
        var gyroPos = JoyconController.Instance.GetGyro(number);
        //joyconlibのジャイロがなぜかz軸とx軸が違うの入れ替えています（持ち方を間違えている可能性あり
        transform.position += new Vector3(gyroPos.z, gyroPos.y, 0f);

        AlignmenPosLimit();
    }*/

    //ジョイコンの動きに制限をかける変数
    [SerializeField] float joyconPosLImit = 0.03f;
    //感度
    //1~0
    [SerializeField] private float sensitivity;
    private void JoyconMove()
    {
        //ベクトル作成
        var gyroPos = JoyconController.Instance.GetGyro(number);
        //ここで手ブレのような微細な振動を入れないようにしています（未検証
        if (gyroPos.magnitude <= joyconPosLImit) return;
        gyroPos *= (1 - sensitivity);
        //joyconlibのジャイロがなぜかz軸とx軸が違うの入れ替えています（持ち方を間違えている可能性あり
        transform.position += new Vector3(gyroPos.z, gyroPos.y, 0f);
        AlignmenPosLimit();
    }

    //可動制限メッソド
    private void AlignmenPosLimit()
    {
        var rectPos = reTf.position;
        rectPos.x = Mathf.Clamp(rectPos.x, min.x, max.x);
        rectPos.y = Mathf.Clamp(rectPos.y, min.y, max.y);
        reTf.position = rectPos;
    }

    //rayを使用して当たり判定を取ってます。リロード時間などを設ける予定
    public void Shot()
    {
        //rayの作成
        //当たり判定を太くする可能性あり
        var shotVec = transform.position - Camera.main.transform.position;
        var shotRay = new Ray(transform.position, shotVec);
        RaycastHit hit;

        //打つ
        if (Physics.Raycast(shotRay, out hit))
        {
            Target hitedTarget = hit.collider.GetComponent<Target>();
            if (hitedTarget == null)
            {
                return;
            }

            TwoHandredPoint hitedTwohandredPoint = hit.collider.GetComponent<TwoHandredPoint>();
            if (hitedTwohandredPoint != null)
            {
                ScoreManager.Instance.SetScore(number, hitedTwohandredPoint.GetPoint());
                return;
            }
            ScoreManager.Instance.SetScore(number, hitedTarget.GetPoint(gameObject));
        }
    }

    //プレイヤーが向くベクトルをつくて送るメソッドです。動きがキモいので消す可能性あり
    private void Look()
    {
        //ベクトル作成
        var alignmentCameraVec = transform.position - Camera.main.transform.position;
        var lookPos = alignmentCameraVec.normalized * 5f + transform.position;
        lookPos.y = 0f;

        PlayerManager.Instance.GetPlayer(number).SetLook(lookPos);
    }
}
