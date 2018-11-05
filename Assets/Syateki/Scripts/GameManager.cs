using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シングルトンクラス
namespace Syateki{
    
    public class GameManager : BestPracticeSingleton<GameManager>
    {
        //ターゲットのムーブを管理するためのbool
        private bool canStarting = false;
        private Timer timer;
        private Bgm bgm;
        [SerializeField] private bool joyconMode = false;
        private int playablePersons;

        public int PlayabelePersons{ get { return playablePersons; }}
        public bool JoyconMode { get { return joyconMode; }}

        private void Awake()
        {
            if (joyconMode)
            {
                JoyconController.Instance.Init();
                playablePersons = JoyconController.Instance.GetJoyconLength();
            }
            else
            {
                JoyconController.Instance.gameObject.SetActive(false);
                playablePersons = 1;
                Debug.Log("joyconを使いたい場合はヒエラルキーのGameManagerのjoyconModeにチェックを入れてください。");
            }
        }

        void Start () {
            //カウントダウンを呼び出しています。
            var countdown = FindObjectOfType<CountDown>();
            
            countdown.Init();
            countdown.CountStart();
            
            timer = FindObjectOfType<Timer>();

            timer.End += () => countdown.EndProcess();

            bgm = FindObjectOfType<Bgm>();
        }
        
        
        // Update is called once per frame
        void Update () {
            if (canStarting != true) return;
            
            //ここからタイマーのカウントダウンを呼び出しいます
            if(timer.TotalTime > 0)timer.Count();
        }

        public void GameStart(){
            TargetManger.Instance.TargetMoving();
            bgm.PlayBgm();
            canStarting = true;
        }

        public void End(){
            bgm.EndBgm();
            ScoreManager.instance.AddRank();
            var rc = FindObjectOfType<RankController>();
            rc.GetRank();
        }
    }
}
    