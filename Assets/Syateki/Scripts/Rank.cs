using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syateki;
using UnityEngine.UI;

namespace Syateki{
    
    public class Rank : MonoBehaviour {
        
        [SerializeField] private int number;
        
        public int Number{ get { return number; }}
        
        public void RankWrite(int rank){
            GetComponent<Text>().text = rank.ToString("") + "位";
        }
    }
}
