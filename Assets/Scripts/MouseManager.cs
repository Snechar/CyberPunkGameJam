using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    private Sprite Sprite;
    private SpriteRenderer Renderer;
    private Plantable storedSteed;

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
                        RemoveMouseItem();
                        SO_Produce produce = potManager.Harvest();
                        InventoryManagerSingleton.Instance.AddItem(produce);
                    }
                    else
                    {
                        if (storedSteed != null)
                        {
                            if (storedSteed.GetPlant().ProduceType == ProduceType.Plant)
                            {
                                potManager.PlantSeed(storedSteed);
                                RemoveMouseItem();
                            }

                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "FishTank")
                {
                    PotManager potManager = hit.collider.gameObject.GetComponent<PotManager>();
                    if (potManager.GetHarvestStatus())
                    {
                        RemoveMouseItem();
                        SO_Produce produce = potManager.Harvest();
                        InventoryManagerSingleton.Instance.AddItem(produce);
                    }
                    else
                    {
                        if (storedSteed != null)
                        {
                            if (storedSteed.GetPlant().ProduceType == ProduceType.Fish)
                            {
                                potManager.PlantSeed(storedSteed);
                                RemoveMouseItem();
                            }
                        }
                    }
                }

            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            RemoveMouseItem();
        }
    }

    public void StoreSeed(Plantable seed)
    {
        storedSteed = seed;
        Renderer.sprite = seed.Sprite;
    }


    private void RemoveMouseItem()
    {
        storedSteed = null;
        Renderer.sprite = null;
    }
}
