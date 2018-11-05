using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;

//シングルトンクラス
namespace Syateki{
    
    public class ScoreManager : BestPracticeSingleton<ScoreManager>
    {
        //プレイヤーのhp管理用の配列
        private int[] scores;
        private List<Score> scoresList;
        private int[] ranks;
        
        private void Awake()
        {
            scores = new int[GameManager.Instance.PlayabelePersons];
            for (int i = 0; i < scores.Length; i++) scores[i] = 0;
            scoresList = new List<Score>();
            ranks = new int[scores.Length];
        }

        private void Start()
        {
            //初期化
            var scoresArray = new Score[Constants.GameSetting.MAX_PERSONS];
            
            foreach (Transform child in transform)
            {
                
                var score = child.GetComponent<Score>();
                
                if (score == null)
                {
                    Debug.Log(child.name + "にScoreがアタッチされていません");
                    return;
                }
                
                scoresArray[score.Number - 1] = score;
            }
            
            //プレイヤーの操作人数の変化に対応するために入れています
            scoresList.AddRange(scoresArray);
            
            //プレイヤーの操作人数の変化に対応するために入れています
            for (int i = GameManager.Instance.PlayabelePersons; i < Constants.GameSetting.MAX_PERSONS; i++)
            {
                scoresList[GameManager.Instance.PlayabelePersons].gameObject.SetActive(false);
                scoresList.RemoveAt(GameManager.Instance.PlayabelePersons);
            }
            
        }
        
        public void SetScore(int number, int point){
            //ここでプレイヤーのnmberを元eに配列に代入しています
            scores[number - 1] += point;
            GetScore(number);
        }
        
        //UI(score)に送るためのメソッド
        private void GetScore(int number){
            scoresList[number - 1].SetScoreUI(scores[number - 1]);
        }
        
        public void AddRank(){
            int i, j;
            for (i = 0; i < ranks.Length; i++) ranks[i] = 1;
            for (i = 0; i < ranks.Length - 1; i++)
            {
                for (j = i + 1; j < ranks.Length; j++)
                {
                    if (scores[i] < scores[j]) ranks[i]++;
                    else if (scores[i] > scores[j]) ranks[j]++;
                }
            }
            
        }
        
        public bool SortArray(int x){
            for (int i = 0; i < scores.Length; i++) if (ranks[i] == x) return true;
            return false;
        }
        
        public int GetRank(int number){
            return ranks[number - 1];
        }
    }
}