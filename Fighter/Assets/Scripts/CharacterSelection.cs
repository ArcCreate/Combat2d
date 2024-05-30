using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    //refrences

    //variables
    private int player1Selection, player2Selection;
    public List<GameObject> player1List = new List<GameObject>();
    public List<GameObject> player2List = new List<GameObject>();


    void Start()
    {
        player1Selection = PlayerPrefs.GetInt("player1Selection");
       // player2Selection = PlayerPrefs.GetInt("player2Selection");
    }
}
