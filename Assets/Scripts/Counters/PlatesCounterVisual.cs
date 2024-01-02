using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private PlatesCounter counter;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGOList;

    private void Awake()
    {
        plateVisualGOList = new List<GameObject>();
    }
    private void Start()
    {
        counter.OnPlateSpawned += OnPlateSpawned_SpawnPlateVisual;
        counter.OnPlateTaken += OnPlateTaken_DespawnPlateVisual;
    }

    private void OnPlateSpawned_SpawnPlateVisual(object sender, System.EventArgs e)
    {
        Debug.Log("Spawning Visual");
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGOList.Count, 0);
        plateVisualGOList.Add(plateVisualTransform.gameObject);
    }

    private void OnPlateTaken_DespawnPlateVisual(object sender, System.EventArgs e)
    {
        Debug.Log("Despawning Visual");
        GameObject plateGameObject = plateVisualGOList.Last();
        plateVisualGOList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
}
