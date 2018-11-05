using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Syateki;


public class PlayerManager : BestPracticeSingleton<PlayerManager>
{
    private List<Player> playersList;

    private void Awake()
    {
        playersList = new List<Player>();
    }
    void Start()
    {
        var playersArray = new Player[Constants.GameSetting.MAX_PERSONS];

        foreach (Transform child in transform)
        {

            var player = child.GetComponent<Player>();

            if(player == null){
                Debug.Log(child.name + "にPlayerがアタッチされていません");
                return;
            }

            playersArray[player.Number - 1] = player;
        }

        //プレイヤーの操作人数の変化に対応するために入れています
        playersList.AddRange(playersArray);

        //プレイヤーの操作人数の変化に対応するために入れています。
        for (int i = GameManager.Instance.PlayabelePersons; i < Constants.GameSetting.MAX_PERSONS; i++)
        {
            playersList[GameManager.Instance.PlayabelePersons].gameObject.SetActive(false);
            playersList.RemoveAt(GameManager.Instance.PlayabelePersons);
        }
              
        //一つ目の照準の初期位置
        var alignmentPos = -3f;

        //照準を出すための位置をplayerに送っています
        foreach (var p in playersList)
        {
            p.InstantAlignment(Vector3.right * alignmentPos);
            //照準の初期位置をずらしています。
            alignmentPos += 2f;
        }
    }

    public Player GetPlayer(int number)
    {
        return playersList[number - 1];
    }
}
