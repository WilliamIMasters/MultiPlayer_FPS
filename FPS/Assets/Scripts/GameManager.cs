using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager Instance;
    PhotonView PV;

    [SerializeField]
    ScoreBoard scoreBoard;

    [SerializeField]
    Dictionary<string, int> playerScores;

    [SerializeField]
    PlayerManager[] players;

    [SerializeField]
    PlayerManager localPlayerManager;

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

        SceneManager.sceneLoaded += NewSceneLoaded;
    }

    private void Start()
    {
        UpdatePlayerManagerList();

        //spawn player manager
        //localPlayerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerManager"), Vector3.zero, Quaternion.identity);
        //Debug.Log("Created localPlayerManager");
    }


    public void Initilise()
    {

        // Set initial score of  every player to 0 on the host, host then updates all other users
        if(PV.Owner == PhotonNetwork.LocalPlayer) {
            playerScores = new Dictionary<string, int>();
            foreach (Player p in PhotonNetwork.PlayerList) {
                playerScores[p.UserId] = 0;
            }

            PV.RPC("RPC_setPlayerScores", RpcTarget.All, playerScores);
        }
    }

    
    void NewSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) { // pregame lobby

            // Generate/get array of level build index for playlist of maps to be played
            // Randomly order list
            // load first level



            CreateLocalPlayerManager();

            // Temporary
            //SceneManager.LoadScene(2);

        } else if(scene.buildIndex > 1) { // playable Level Loaded
            if(!localPlayerManager) {
                CreateLocalPlayerManager();
                UpdatePlayerManagerList();
            }

            // Local player is spawned on each client
            localPlayerManager.Spawn();

        }
    }

    void CreateLocalPlayerManager()
    {
        localPlayerManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerManager"), Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();
        localPlayerManager.PlayerDied.AddListener(playerDied);
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
            if (!playerScores.ContainsKey(p.UserId)) {
                playerScores[p.UserId] = 0;
            }
            scoreBoard.AddScoreboardItem(p, playerScores[p.UserId]);
        }
    }

    public void UpdatePlayerManagerList()
    {
        Debug.Log("Player list length: " + PhotonNetwork.PlayerList.Length);
        GameObject[] playerManagerObjects = GameObject.FindGameObjectsWithTag("PlayerManager");
        Debug.Log("playerManagerObjects length: " + playerManagerObjects.Length);
        players = new PlayerManager[playerManagerObjects.Length];
        for(int i=0; i < playerManagerObjects.Length; i++) {
            players[i] = playerManagerObjects[i].GetComponent<PlayerManager>();
        }
    }


    public void SpawnAllPlayers()
    {
        if(players != null) {
            foreach(PlayerManager p  in players) {
                p.Spawn();
            }
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //UpdatePlayerManagerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //UpdatePlayerManagerList();
    }

    public void playerDied()
    {
        PV.RPC("RPC_playerDied", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_playerDied()
    {
        Debug.Log("Player died");
        if(PV.Owner == PhotonNetwork.LocalPlayer) {

            // Gets total number of players who are  still alive
            int aliveTally = 0;
            PlayerManager lastAliveFound=null;
            for(int i=0; i < players.Length; i++) {
                if (players[i].alive) {
                    aliveTally++;
                    lastAliveFound = players[i];
                }
            }
            if(aliveTally == 1 && lastAliveFound != null) {
                Debug.LogError(lastAliveFound.nickName + " is the last alive");
            }
            
        }
    }

}
