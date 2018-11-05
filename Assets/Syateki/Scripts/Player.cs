using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syateki{
    
    public class Player : MonoBehaviour {
        
        //何番のプレイヤーなのかを区別するための変数
        [SerializeField] private int number;
        [SerializeField] private Alignment alignment;

        public int Number { get { return number; }}

        //ここで照準を生成しています
        public void InstantAlignment(Vector3 alignemntPos)
        {
            var go = Instantiate(alignment, alignemntPos, Quaternion.identity);

            //関連づけるための情報を送っています
            go.Number = number;
            go.Init();

            var child = transform.Find("Gun").gameObject.GetComponent<Gun>();

            if(child == null){
                Debug.Log("Gunが見つからないまたは、Gunがアタッチされていません" + gameObject.name + "を確認してください");
                return;
            }
            AlignmentContorller.Instance.SetAlignment(number, go);
            child.Init();
        }

        public void SetLook(Vector3 lookPos){
            transform.LookAt(lookPos);
        }
    }
}