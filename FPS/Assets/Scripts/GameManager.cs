using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    PhotonView PV;

    [SerializeField]
    ScoreBoard scoreBoard;

    [SerializeField]
    Dictionary<string, int> playerScores;

    private void Awake()
    {
        if (Instance) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        PV = GetComponent<PhotonView>();
        Initilise();
    }

    public void Initilise()
    {
        if(PV.Owner == PhotonNetwork.LocalPlayer) {
            playerScores = new Dictionary<string, int>();
            foreach (Player p in PhotonNetwork.PlayerList) {
                playerScores[p.UserId] = 0;
            }

            PV.RPC("RPC_setPlayerScores", RpcTarget.All, playerScores);
        }
    }

    

    [PunRPC]
    void RPC_setPlayerScores(Dictionary<string, int> scores)
    {
        playerScores = scores;
        updateScoreBoard();
    }

    public void increasePlayerScore(Player player)
    {
        if (PV.Owner == PhotonNetwork.LocalPlayer) {
            playerScores[player.UserId]++;
            PV.RPC("RPC_setPlayerScores", RpcTarget.All, playerScores);
        }

    }


    public void updateScoreBoard()
    {
        Debug.Log("updateScoreBoard()");
        
        scoreBoard.ClearBoard();
        
        foreach(Player p in PhotonNetwork.PlayerList) {
            scoreBoard.AddScoreboardItem(p, playerScores[p.UserId]);
        }
        //scoreBoard.UpdateScoreBoard(playerScores);
    }

    
}
