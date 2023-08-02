using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned = delegate { };
    public event EventHandler OnplateRemoved = delegate { };

    [SerializeField] private KitchenObjectsSO plateKitchenObjectSo;

    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _plateSpawnedAmount;
    private int _plateSpawnedAmountMax = 4;

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;

        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if (_plateSpawnedAmount < _plateSpawnedAmountMax)
            {
                _plateSpawnedAmount++;

                OnPlateSpawned.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact (Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is empty handed
            if (_plateSpawnedAmount > 0)
            {
                _plateSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);
                OnplateRemoved.Invoke(this, EventArgs.Empty);
            }
        }
    }
}