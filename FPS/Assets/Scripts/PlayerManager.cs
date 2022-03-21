using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    ScoreBoard scoreBoard;

    Player player;

    PhotonView PV;
    GameObject controller;

    public UnityEvent PlayerDied = new UnityEvent();

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        player = PhotonNetwork.LocalPlayer;
        
    }

    private void Start()
    {
        scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<ScoreBoard>() ;
        if (scoreBoard == null) {
            Debug.LogError("Coundnt find scoreboard");
        }


        if (PV.IsMine) {
            CreateController();

            

        }

        
    }

    private void Update()
    {
        
    }

    void CreateController()
    {
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();

        Debug.Log("Instantiated player Controller");
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);

        CreateController();
    }

}
