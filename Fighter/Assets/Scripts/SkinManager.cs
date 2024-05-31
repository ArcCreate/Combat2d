using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    
    //refrences
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRenderer2;
    public List<Sprite> skins = new List<Sprite>();
    public List<Sprite> skins2 = new List<Sprite>();
    private int selectedSkin = 0;
    private int selectedSkin2 = 0;

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

    public void Next2()
    {
        selectedSkin2 = selectedSkin2 + 1;
        if (selectedSkin2 == skins2.Count)
        {
            selectedSkin2 = 0;
        }
        spriteRenderer2.sprite = skins2[selectedSkin2];
    }

    public void Back2()
    {
        Debug.Log("back");
        selectedSkin2 = selectedSkin2 - 1;
        if (selectedSkin2 < 0)
        {
            selectedSkin2 = skins2.Count - 1;
        }
        spriteRenderer2.sprite = skins2[selectedSkin2];
    }

    public void Play()
    {
        PlayerPrefs.SetInt("player1Selection", selectedSkin);
        PlayerPrefs.SetInt("player2Selection", selectedSkin2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
