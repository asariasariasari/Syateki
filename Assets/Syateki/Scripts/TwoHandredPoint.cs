using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;

public class TwoHandredPoint : Target{
    
    public override void Move(){
        StartCoroutine(Moving(limitPos_y));
    }
}
