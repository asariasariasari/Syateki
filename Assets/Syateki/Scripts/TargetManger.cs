using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;

//シングルトンクラス
public class TargetManger : BestPracticeSingleton<TargetManger>{
    
    //シーン上全てのターゲットを保持するリスト
    private List<Target> targets;
    private List<Target> moveTargets;
    //動かすターゲットを決める変数
    private int length = 4;
    [SerializeField] private Target[] instantTargets;
    private TwoHandredPoint twoHadoredTarget;

    private Timer timer;

    private void Awake()
    {
        targets = new List<Target>();
        moveTargets = new List<Target>();
    }
    private void Start()
    {
        twoHadoredTarget = FindObjectOfType<TwoHandredPoint>();
        //時間が０になった時に止めるために参照していますが汚いので直したいところ
        timer = FindObjectOfType<Timer>();

        StartCreateTarget();
                    
        foreach (Transform child in transform)
        {
            targets.Add(child.GetComponent<Target>());
        }
        moveTargets = targets.OrderBy(i => Guid.NewGuid()).Take(length).ToList();
        StartCoroutine(LengthUP());
    }

    private void StartCreateTarget(){
        float z = 2.4f;
        for (float y = 1.05f ; z <= 4.65f; y += 1.85f){
            for (float x = -4.5f; x <= 4.5f; x += 1.5f){
                var t = Instantiate(instantTargets[2], new Vector3(x, y, z), Constants.TargetSetting.INSTANTTARGET_ROTASION);
                t.transform.SetParent(transform, false);
            }
            z++;
        }
    }

    public void TargetMoving(){
        //ランダムに 並び替えています
        foreach(var t in moveTargets){
            t.Move();
        }
    }

    public void moveTwoHandredTarget(){
        twoHadoredTarget.Move();
    }

    public void InstantTarget(Target target){
        if (timer.TotalTime <= Constants.TargetSetting.NORMALTARGET_ENDTIME) return;
        targets.Remove(target);
        moveTargets.Remove(target);
        var t = Instantiate(RandomInstant(), target.StartPos, Constants.TargetSetting.INSTANTTARGET_ROTASION);
        t.transform.SetParent(transform, false);
        targets.Add(t);
        MoveTarget();
    }

    public void MoveEnd(Target target){
        if (timer.TotalTime <= Constants.TargetSetting.NORMALTARGET_ENDTIME) return;
        moveTargets.Remove(target);
        MoveTarget();
    }

    private void MoveTarget(){
        while(moveTargets.Count() <= length)
        {
            var t = targets[UnityEngine.Random.Range(0, targets.Count())];
            if (moveTargets.Contains(t)) continue;
            moveTargets.Add(t);
            t.Move();
        }
        
    }
    private IEnumerator LengthUP(){
        while(timer.TotalTime >= 0){
            yield return new WaitForSeconds(Constants.TargetSetting.TARGETUP_TIME);
            if (length >= Constants.TargetSetting.LENGTH_MAX) yield break;
            length++;
        }
    }

    private Target RandomInstant(){
        var random = UnityEngine.Random.Range(0, 100);
        if (0 <= random && random < 10) return instantTargets[0];
        if (10 <= random && random < 30) return instantTargets[1];
        return instantTargets[2];
    }
}
