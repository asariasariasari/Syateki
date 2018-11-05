using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Syateki{
    
    public class Score : MonoBehaviour {
        
        //どのプレイヤーのスコアかを認識するための変数
        [SerializeField] private int number;
        
        public int Number{ get { return number; }}
        private Text scoreText;
        
        void Start () {
            scoreText = GetComponent<Text>();
        }
        
        public void SetScoreUI(int score){
            scoreText.text = score.ToString("");
        }
    }
}