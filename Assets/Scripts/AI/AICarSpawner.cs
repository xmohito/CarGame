using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject[] carAIPrefabs;
    GameObject[] carAIPool = new GameObject[20];


    Transform playerCarTransform;

    float timeLastCarSpawned = 0;
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    


    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;
        for (int i = 0; i < carAIPool.Length; i++)
        {
            carAIPool[i] = Instantiate(carAIPrefabs[prefabIndex]);
            carAIPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex >= carAIPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }
        StartCoroutine(UpdateLEssOftenCO());
    }

 IEnumerator UpdateLEssOftenCO()
 {
    while (true)
    {
        CleanUpCarsBeyondView();
        SpawnNewCars();
        yield return wait;
    }
 }

    void SpawnNewCars()
    {
        if(Time.time - timeLastCarSpawned < 2)
        {
            return;
        }

        GameObject carToSpawn = null;
        foreach (GameObject aiCar in carAIPool)
        {
            if (aiCar.activeInHierarchy)
            {
                continue;
            }

            carToSpawn = aiCar;
            break;

        }

        if (carToSpawn == null)
        {
            return;
        }

        Vector3 spawnPosition = new Vector3(0, 0, playerCarTransform.position.z + 100);

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);




        timeLastCarSpawned = Time.time;
        
    }

    void CleanUpCarsBeyondView()
    {
        foreach (GameObject aiCar in carAIPool)
        {
            if(!aiCar.activeInHierarchy)
            {
                continue;
            }

            if(aiCar.transform.position.z - playerCarTransform.position.z > 200)
            {
                aiCar.SetActive(false);
            }

            if (aiCar.transform.position.z - playerCarTransform.position.z < -50)
            {
                aiCar.SetActive(false);
            }

        }
    }
}
