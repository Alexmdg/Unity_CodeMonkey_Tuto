using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;

    [SerializeField] private KitchenObjectSO platesSO;
    private float spawnPlatesTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlatesTimer += Time.deltaTime;
        if (spawnPlatesTimer > spawnPlateTimerMax)
        {
            SpawnPlate();
        }
    }

    private void SpawnPlate()
    {
        if(platesSpawnedAmount < platesSpawnedAmountMax ) 
        {
            Debug.Log("Firing Plate Event");
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            platesSpawnedAmount++;
        }
        spawnPlatesTimer = 0f;
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) 
        {
            if (platesSpawnedAmount > 0) 
            {
                platesSpawnedAmount--;
                OnPlateTaken?.Invoke(this, EventArgs.Empty);
                KitchenObject.SpawnKitchenObject(platesSO, player);
            }
        }
    }
}
