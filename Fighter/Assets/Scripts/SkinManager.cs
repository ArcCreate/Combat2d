using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    
    //refrences
    public SpriteRenderer spriteRenderer;
    public List<Sprite> skins = new List<Sprite>();
    private int selectedSkin = 0;

    public void Next()
    {
        selectedSkin = selectedSkin + 1;
        if( selectedSkin == skins.Count)
        {
            selectedSkin = 0;
        }
        spriteRenderer.sprite = skins[selectedSkin];
    }

    public void Back()
    {
        Debug.Log("back");
        selectedSkin = selectedSkin - 1;
        if (selectedSkin < 0)
        {
            selectedSkin = skins.Count - 1;
        }
        spriteRenderer.sprite = skins[selectedSkin];
    }

    public void Play()
    {
        PlayerPrefs.SetInt("player1Selection", selectedSkin);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
