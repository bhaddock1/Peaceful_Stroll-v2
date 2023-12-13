using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameManager gameManager;
    private PlayerController playerControllerScript;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2;
    private float repeatRate = 2;

    public bool gameOver { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {

        if (!gameManager.DebugToggle)
        {
            InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        }
    }

    // Update is called once per frame
    private void SpawnObstacle()
    {
        if(!GameManager.gameOver)
        {
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
        else
        {
            CancelInvoke();
        }
    }
}
