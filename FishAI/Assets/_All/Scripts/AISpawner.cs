using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


[System.Serializable]
public class AIObjects
{
    //Declare Variables
    public string AIGroupName { get { return m_aiGroupName; } }
    public GameObject objectPrefab { get { return m_prefab; } }
    public int maxAI { get { return m_maxAI; } }
    public int spawnRate { get { return m_spawnRate; } }
    public int spawnAmount { get { return m_maxSpawnAmount; } }
    public bool randomizeStats { get { return m_randomizeStats; } }
    public bool enableSpawner { get { return m_enableSpawner; } }

    [SerializeField]
    [Header("AI Group Stats")]
    private string m_aiGroupName;
    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    [Range(0f, 40f)]
    private int m_maxAI;
    [SerializeField]
    [Range(0f, 20f)]
    private int m_spawnRate;
    [SerializeField]
    [Range(0f, 10f)]
    private int m_maxSpawnAmount;
    
    
    //new variables
    [Header("Main Settings")]
    [SerializeField]
    public bool m_enableSpawner;
    [SerializeField]
    private bool m_randomizeStats;


    public AIObjects(string Name, GameObject Prefab, int MaxAI, int SpawnRate, int SpawnAmount, bool RandomizeStats)
    {
        this.m_aiGroupName = Name;
        this.m_prefab = Prefab;
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
        this.m_randomizeStats = RandomizeStats;
    }

    public void setValues(int MaxAI, int SpawnRate, int SpawnAmount)
    {
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
    }

}

public class AISpawner : MonoBehaviour
{

    //Declare Variables

    //note: using list because we don't know the size of it, array would need to set size first
    public List<Transform> Waypoints = new List<Transform>();

    public float spawnTimer { get { return m_SpawnTimer; } } //global value for how often we run the spawner
    public Vector3 spawnArea { get { return m_SpawnArea; } }

    //Serialize the private variables
    [Header("Global Stats")]
    [Range(0f, 600f)]
    [SerializeField]
    public float m_SpawnTimer;  //global value for how often we run the spawner
    [SerializeField]
    private Color m_SpawnColor = new Color(1.000f, 0.000f, 0.000f, 0.300f); //use the color for the gizmo - set nice red default
    [SerializeField]
    private Vector3 m_SpawnArea = new Vector3 (20f, 10f, 20f);



    //Create array from new class
    [Header("AI Groups Settings")]
    public AIObjects[] AIObject = new AIObjects[5];



    // Use this for initialization
    void Start()
    {
        GetWaypoints();
        RandomizeGroups();
        CreateAIGroups();
        InvokeRepeating("SpawnNPC", 0.5f, spawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnNPC()
        {
            //loop through all of the AI Groups
            for (int i = 0; i < AIObject.Count(); i++)
            {
                //check to make sure spawner is enabled
                if(AIObject[i].enableSpawner && AIObject[i].objectPrefab != null)
                {
                    //make sure that AI Group doesn't have max NPCs
                    GameObject tempGroup = GameObject.Find(AIObject[i].AIGroupName);
                    if(tempGroup.GetComponentInChildren<Transform>().childCount < AIObject[i].maxAI)
                    {
                            //spawn random number of NPCs from 0 to Max Spawn Amount
                            for (int y = 0; y < Random.Range(0,AIObject[i].spawnAmount); y++)
                                {
                                    //get random rotation
                                    Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
                                    //create spawned gameobject
                                    GameObject tempSpawn;
                                    tempSpawn = Instantiate(AIObject[i].objectPrefab, RandomPosition(), randomRotation);
                                    //put spawned NPC as child of group
                                    tempSpawn.transform.parent = tempGroup.transform;
                                    //Add the AIMove script and class to the new NPC
                                    tempSpawn.AddComponent<AIMove>();
                                }
                    
                    }
                }
            }
        }

    //public method for Random Position within the Spawn Area
    public Vector3 RandomPosition()
    {
        //get a random position within our Spawn Area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
            );
        randomPosition = transform.TransformPoint(randomPosition * .5f);
        return randomPosition;
    }

    //public method for getting a random Waypoint
    public Vector3 RandomWaypoint()
    {
        int randomWP = Random.Range(0, (Waypoints.Count - 1));
        Vector3 randomWaypoint = Waypoints[randomWP].transform.position;
        return randomWaypoint;
    }


    //Method for putting random values in the AI Group Settings
    void RandomizeGroups()
    {
        //Randomize
        for (int i = 0; i < AIObject.Count(); i++)
        {
            if (AIObject[i].randomizeStats)
            {
                //AIObject[i].maxAI = Random.Range(1, 30);
                //AIObject[i] = new AIObjects(AIObject[i].AIGroupName, AIObject[i].objectPrefab, Random.Range(1, 30), Random.Range(1, 20), Random.Range(1, 10), AIObject[i].randomizeStats);
                AIObject[i].setValues(Random.Range(1, 30), Random.Range(1, 20), Random.Range(1, 10));
            }
        }
    }

    //Method for creating the empty worldobject groups
    void CreateAIGroups()
    {
        for (int i = 0; i < AIObject.Count(); i++)
        {
            //Empty Game Object to keep our AI in
            GameObject AIGroupSpawn;

            //create a new game object
            AIGroupSpawn = new GameObject(AIObject[i].AIGroupName);
            AIGroupSpawn.transform.parent = this.gameObject.transform;
        }
    }

    void GetWaypoints()
    {
        //List using standar library
        //look through nested children
        Transform[] wpList = this.transform.GetComponentsInChildren<Transform>(); //note: 'this' is not required anymore
        for (int i=0; i < wpList.Length; i++)
        {
            if(wpList[i].tag == "waypoint") 
            {
                //add to the list
                Waypoints.Add(wpList[i]);
            }
        }
    }
    //show the gizmos in colour
    void OnDrawGizmosSelected()
    {
        Gizmos.color = m_SpawnColor;
        Gizmos.DrawCube(transform.position, spawnArea);
    }
}


