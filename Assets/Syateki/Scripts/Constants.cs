using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syateki{

    //定数クラスなので継承していません
    public static class Constants{

        public static class GameSetting{
            //プレイ可能な最大人数
            public static readonly int MAX_PERSONS = 4;
        }
       
        public static class TargetSetting{
            //可動制限範囲
            public static readonly float TARGETUP_TIME = 4f;
            public static readonly float LENGTH_MAX = 21;
            public static readonly float NORMALTARGET_ENDTIME = 7f;
            public static readonly Quaternion INSTANTCRASHTARGET_ROTASION = Quaternion.Euler(96f, -90f, 0);
            public static readonly Quaternion INSTANTTARGET_ROTASION = Quaternion.Euler(0, 180f, -6f);
        }

        public static class GunSetting{
            public static readonly float INTERVAL_TIME = 0.5f;
        }
    }
}
