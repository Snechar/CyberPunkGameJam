using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    Fish,
    Plant,
}

public class ChangeList : MonoBehaviour
{

    public List<GameObject> plantList = new List<GameObject>();
    public List<GameObject> fishList = new List<GameObject>();

    public Sprite plantSprite = null;
    public Sprite fishSprite = null;

    public UIState state = UIState.Plant;

    public Image spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponent<Image>();
        spriteRenderer.sprite = fishSprite;
    }


    public void SwitchUIState()
    {
        if (state == UIState.Plant)
        {
            foreach (GameObject go in plantList)
            {
                go.SetActive(false);
            }
            foreach (var item in fishList)
            {
                item.SetActive(true);
            }
            spriteRenderer.sprite = plantSprite;
            state = UIState.Fish;

        }
        else
        {

            foreach (GameObject go in plantList)
            {
                go.SetActive(true);
            }
            foreach (var item in fishList)
            {
                item.SetActive(false);
            }
            spriteRenderer.sprite = fishSprite;
            state = UIState.Plant;

        }



    }
}
