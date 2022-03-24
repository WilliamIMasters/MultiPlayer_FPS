using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    [SerializeField]
    ScoreBoard scoreBoard;

    [SerializeField]
    Player player;

    PhotonView PV;
    GameObject controller;

    public UnityEvent PlayerDied = new UnityEvent();

    [SerializeField]
    public string nickName;

    [SerializeField]
    public bool alive;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //player = PhotonNetwork.LocalPlayer;
        player = PV.Owner;

        DontDestroyOnLoad(gameObject);
        gameObject.name = "Player Manager: " + player.NickName;
        nickName = player.NickName;

    }

    private void Start()
    {
        alive = false;

        scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<ScoreBoard>() ;
        if (scoreBoard == null) {
            Debug.LogError("Coundnt find scoreboard");
        }


        if (PV.IsMine) {
            //CreateController();

            

        }

        
    }

    

    void CreateController()
    {
        if (PV.IsMine) {
            alive = true;
            Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();

            Debug.Log("Instantiated player Controller");
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });



        }

    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        alive = false;

        //CreateController();
        PlayerDied.Invoke();
    }

    public void Spawn()
    {
        CreateController();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //gm.UpdatePlayerManagerList();   // Maybe change to add this playermanager to playermanager list instead of just  refreshing it every time

        GameManager.Instance.UpdatePlayerManagerList();
        
    }
}
