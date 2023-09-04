using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    public const string MixerMusic = "MusicVolume";
    public const string MixerSoundEffects = "SfxVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("There is more than one OptionsUI instance!");
        }

        Instance = this;
        
        soundEffectsSlider.onValueChanged.AddListener(volume =>
        {
            SoundManager.Instance.SetVolume(volume);
            SetSoundEffectsVolume(SoundManager.Instance.GetVolume());
        });
        
        backButton.onClick.AddListener(Hide);
    }
    
    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MusicKey, 1f);
        soundEffectsSlider.value = PlayerPrefs.GetFloat(SoundManager.SfxKey, 1f);

        GameManager.Instance.OnGamePausedClose += GameManagerGamePausedClose;
        
        UpdateVisual();
        Hide();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(SoundManager.MusicKey, musicSlider.value);
        PlayerPrefs.SetFloat(SoundManager.SfxKey, soundEffectsSlider.value);
    }

    private void GameManagerGamePausedClose(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = $"Sound Effects: {Mathf.Round(SoundManager.Instance.GetVolume() * 100f).ToString()}";
        musicText.text = $"Music: {Mathf.Round(musicSlider.value * 100f).ToString()}";
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat(MixerMusic, Mathf.Log10(sliderValue) * 20f);
        UpdateVisual();
    }

    public void SetSoundEffectsVolume(float sliderValue)
    {
        audioMixer.SetFloat(MixerSoundEffects, Mathf.Log10(sliderValue) * 20f);
        UpdateVisual();
    }
}