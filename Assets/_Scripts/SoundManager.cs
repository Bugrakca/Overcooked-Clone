using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [SerializeField] private AudioClipRefsSO audioClipRefsSo;
    [SerializeField] private AudioMixer audioMixer;

    public const string SfxKey = "sfxVolume";
    public const string MusicKey = "musicVolume";

    private float _volume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one SoundManager instance!");
        }

        Instance = this;
        
        LoadVolume();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        PlayerSounds.OnPlayerMoveSound += PlayerSounds_OnPlayerMoveSound;
    }

    public void SetVolume(float value)
    {
        _volume = value;
    }

    public float GetVolume()
    {
        return _volume;
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SfxKey, 1f);

        audioMixer.SetFloat(OptionsUI.MixerMusic, Mathf.Log10(musicVolume) * 20f);
        audioMixer.SetFloat(OptionsUI.MixerSoundEffects, Mathf.Log10(sfxVolume) * 20f);
    }

    private void PlayerSounds_OnPlayerMoveSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSo.footstep, Player.Instance.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSo.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaced(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSo.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSo.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSo.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSo.deliverySuccess, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSo.deliveryFail, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * _volume);
    }
}
