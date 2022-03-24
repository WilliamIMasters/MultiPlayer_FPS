using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTester : MonoBehaviour
{

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            gm.increasePlayerScore(PhotonNetwork.LocalPlayer);
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            SceneManager.LoadScene(2);
        }
    }
}
