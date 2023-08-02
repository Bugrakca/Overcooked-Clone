using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> _plateVisualGameObjectList = new List<GameObject>();

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounterOnPlateSpawned;
        platesCounter.OnplateRemoved += PlatesCounterOnplateRemoved;
    }

    private void PlatesCounterOnplateRemoved (object sender, EventArgs e)
    {
        GameObject plateGameObject = _plateVisualGameObjectList[^1];
        _plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounterOnPlateSpawned (object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffset = .1f;
        plateVisualTransform.localPosition = new Vector3(0f, plateOffset * _plateVisualGameObjectList.Count, 0f);
        
        _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
