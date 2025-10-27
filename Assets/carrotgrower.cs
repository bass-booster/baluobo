using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carrotgrower : MonoBehaviour

{
     [Header("Prefabs")]
    public GameObject carrotGrowPrefab;
    public GameObject pullableCarrotPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;
    public float spawnOffsetY = -0.5f;

    private float timer;
    private bool carrotActive = false;

    void Update()
    {
        if (carrotActive) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCarrotFromDirt();
            timer = 0f;
        }
    }

    void SpawnCarrotFromDirt()
    {
        GameObject[] dirtPatches = GameObject.FindGameObjectsWithTag("Dirt");
        if (dirtPatches.Length == 0) return;

        GameObject dirt = dirtPatches[Random.Range(0, dirtPatches.Length)];
        Vector3 spawnPos = dirt.transform.position + new Vector3(0, spawnOffsetY, 0);

        GameObject grower = Instantiate(carrotGrowPrefab, spawnPos, Quaternion.identity);

        CarrotAnimatorBridge bridge = grower.GetComponent<CarrotAnimatorBridge>();
        if (bridge != null)
        {
            bridge.pullableCarrotPrefab = pullableCarrotPrefab;
            bridge.grower = this;
        }

        carrotActive = true;
    }

    public void NotifyCarrotPulled()
    {
        carrotActive = false;
    }
}