using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    Dictionary<Player, ScoreBoardItem> scoreboardItems = new Dictionary<Player, ScoreBoardItem>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList) {
            AddScoreboardItem(player);
        }
        //hide();

    }

    void AddScoreboardItem(Player player)
    {
        ScoreBoardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreBoardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }

    void OnPlayerEnterRoom(Player newPlayer)
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
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
}
