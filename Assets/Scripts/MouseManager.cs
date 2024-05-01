using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    private Sprite Sprite;
    private SpriteRenderer Renderer;
    private Seed storedSteed;

    public UnityEvent m_MyEvent = new UnityEvent();
    void Start()
    {
       Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = vector2;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Pot")
                {
                    PotManager potManager = hit.collider.gameObject.GetComponent<PotManager>();
                    if (potManager.GetHarvestStatus())
                    {
                        RemoveSeed();

                    }
                    else
                    {
                        if (storedSteed != null)
                        {
                            potManager.PlantSeed(storedSteed);
                            RemoveSeed();
                        }
                    }
                }

            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            RemoveSeed();
        }
    }

    public void StoreSeed(Seed seed)
    {
        storedSteed = seed;
        Renderer.sprite = seed.Sprite;
    }

    private void RemoveSeed()
    {
        storedSteed = null;
        Renderer.sprite = null;
    }
}
