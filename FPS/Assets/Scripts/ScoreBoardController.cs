using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{

    public static ScoreBoardController Instance;

    [SerializeField]
    GameObject scoreboard;
    [SerializeField]
    GameObject endGameLeaderboard;
    [SerializeField]
    TMP_Text endGameLeaderboardWinnerText;

    private void Awake()
    {
        if(Instance) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

    }

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
