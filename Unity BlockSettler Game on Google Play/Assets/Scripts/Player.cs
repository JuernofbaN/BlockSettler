using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Player : MonoBehaviour
{
    public int stage;
    public int level;

    public Player(Player player)
    {
        stage = player.stage;
        level = player.level;
    }
    public Player(int st, int lev)
    {
        stage = st;
        level = lev;
    }

    public void SavePlayer()
    {
        SaveSystem.SaveData(this);
    }
    public void LoadPlayer()
    {
        Player data = SaveSystem.LoadData();
        stage = data.stage;
        level = data.level;
    }

    
}
