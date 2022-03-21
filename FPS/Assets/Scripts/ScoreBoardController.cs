using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField]
    GameObject scoreboard;

    /*private void Awake()
    {
        scoreboard = GetComponent<ScoreBoard>();
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab)) {
            //show scoreboard
            scoreboard.SetActive(true);
        } else {
            //hide scoreboard
            scoreboard.SetActive(false);
        }
    }
}
