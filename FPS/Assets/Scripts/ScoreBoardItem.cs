using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardItem : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text scoreText;

    public void Initialize(Player player, int score)
    {
        usernameText.text = player.NickName;
        scoreText.text = score.ToString();
    }

}
