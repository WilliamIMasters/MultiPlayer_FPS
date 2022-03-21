using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    //Dictionary<Player, ScoreBoardItem> scoreboardItems = new Dictionary<Player, ScoreBoardItem>();

    private void Start()
    {
        //foreach (Player player in PhotonNetwork.PlayerList) {
        //    AddScoreboardItem(player);
        //}

        //Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;

        //hide();

    }

    public void UpdateScoreBoard(Dictionary<Player, int> playerScores)
    {
        Player[] allPlayers = PhotonNetwork.PlayerList;

        ClearBoard();
        foreach (Player p in allPlayers) {

            AddScoreboardItem(p, playerScores[p]);

        }
    }

    public void AddScoreboardItem(Player player, int score)
    {
        ScoreBoardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreBoardItem>();
        item.Initialize(player, score);
        
    }

    public void ClearBoard()
    {
        for(int i=0; i < container.childCount; i++) {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    /*void OnPlayerEnterRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }*/

    
}
