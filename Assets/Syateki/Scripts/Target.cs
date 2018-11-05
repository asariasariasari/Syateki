using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Syateki;

public class Target : MonoBehaviour
{
    //ターゲットのポイント数種類を増やす予定
    [SerializeField] private int point = 10;
    [SerializeField] private int strength = 1;
    //最初に打った時しか加点しないためのリスト
    [SerializeField]private float speed = 2f;
    private List<GameObject> players;
    //上げ下げの動きをするための変数
    private float vib = 0;
    //ターゲットの初期位置
    private Vector3 startPos;
    //上がりきった後に止まる時間ポイントに応じて短くする予定
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] protected BreakTarget bk;
    [SerializeField] protected float limitPos_y = 1.45f;
    public Vector3 StartPos{ get { return startPos; }}
    private int totalDamage = 0;
    private Timer timer;

    private void Awake()
    {
        players = new List<GameObject>();
        startPos = transform.position;
    }

    public virtual int GetPoint(GameObject player){
        //ターゲットを打ったプレイヤーをリストに入れて同じターゲットは最初しか加点できないようにしています.
        if (players.Contains(player)) return 0;
        players.Add(player);
        if (players.Count() == strength) Break();
        return point;
    }

    public int GetPoint(){
        totalDamage += point;
        if (totalDamage >= 200) Break();
        return point;
    }

    private void Break(){
        if (bk == null) return;
        Instantiate(bk, transform.position, Constants.TargetSetting.INSTANTCRASHTARGET_ROTASION);
        TargetManger.Instance.InstantTarget(gameObject.GetComponent<Target>());
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        StartCoroutine(Starting());
    }

    //ピストン運動させるためのメソッド
    //三角関数のようなイメージ
    protected IEnumerator Moving(float limitPos){
        while (vib < limitPos)
        {
            vib += Time.deltaTime * speed;
            if (vib > limitPos){
                vib = limitPos;
            }
            transform.position = startPos + (Vector3.up * Mathf.PingPong(vib, limitPos_y));
            yield return null;
        }
    }

    IEnumerator Starting()
    {
        yield return StartCoroutine(Moving(limitPos_y));

        //止まる時間
        yield return new WaitForSeconds(waitTime);

        yield return StartCoroutine(Moving(limitPos_y * 2));

        //初期化
        vib = 0;

        players.Clear();
        //ピストン運動が完了したことを報告するため
        //if(hasBreken)
        TargetManger.Instance.MoveEnd(gameObject.GetComponent<Target>());
    }
}