using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemToSpawn
{
    public GameObject item;
    public float spawnRate;
    [HideInInspector] public float minSpawnRate, maxSpawnRate;
}


public class FishClickAndDestroy : MonoBehaviour
{
    
    public float expGranted;
    public ItemToSpawn[] itemToSpawn;
    private GameObject[] players;
    //private GameObject[] loots;
    private int food = 1;
    private Text foodText;
    private int gems = 1;
    private Text gemText;

    void Start()
    {
        //Find Player Tag
        players = GameObject.FindGameObjectsWithTag("Player");
        foodText = UIController.instance.transform.Find("foodImage/foodText").GetComponent<Text>();
        gemText = UIController.instance.transform.Find("gemImage/gemText").GetComponent<Text>();
    }

   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                BoxCollider bc = hit.collider as BoxCollider;
                if (bc != null)
                {
                    //Destroy Object
                    Destroy(bc.gameObject);

                    //Give Loots
                    DropFood();
                    //DropGems();

                    //Give Experience
                    foreach (GameObject go in players)
                    {
                        go.GetComponent<PlayerController>().SetExperience(expGranted / players.Length);
                    }
                }
            }
        }

        
    }
    public void DropFood()
    {
        //print("LootDrop");
        for (int i = 0; i < itemToSpawn.Length; i++)
        {

            if (i == 0)
            {
                itemToSpawn[i].minSpawnRate = 0;
                itemToSpawn[i].maxSpawnRate = itemToSpawn[i].spawnRate - 1;
            }
            else
            {
                itemToSpawn[i].minSpawnRate = itemToSpawn[i - 1].maxSpawnRate + 1;
                itemToSpawn[i].maxSpawnRate = itemToSpawn[i].minSpawnRate + itemToSpawn[i].spawnRate - 1;
            }
        }

        float randomNum = Random.Range(0, 100);

        for (int i = 0; i < itemToSpawn.Length; i++)
        {
            if (randomNum >= itemToSpawn[i].minSpawnRate && randomNum <= itemToSpawn[i].maxSpawnRate)
            {
                GetFood();
            }

        }

    }

    //public void DropGems()
    //{
    //    //print("LootDrop");
    //    for (int i = 0; i < itemToSpawn.Length; i++)
    //    {

    //        if (i == 0)
    //        {
    //            itemToSpawn[i].minSpawnRate = 0;
    //            itemToSpawn[i].maxSpawnRate = itemToSpawn[i].spawnRate - 1;
    //        }
    //        else
    //        {
    //            itemToSpawn[i].minSpawnRate = itemToSpawn[i - 1].maxSpawnRate + 1;
    //            itemToSpawn[i].maxSpawnRate = itemToSpawn[i].minSpawnRate + itemToSpawn[i].spawnRate - 1;
    //        }
    //    }

    //    float randomNum = Random.Range(0, 100);

    //    for (int i = 0; i < itemToSpawn.Length; i++)
    //    {
    //        if (randomNum >= itemToSpawn[i].minSpawnRate && randomNum <= itemToSpawn[i].maxSpawnRate)
    //        {
    //            GetGems();
    //        }

    //    }

    //}

    public void GetFood()
    {
        food++;
        foodText.text = food.ToString("0");
    }

    public void GetGems()
    {
        gems++;
        gemText.text = gems.ToString("0");
    }

}
