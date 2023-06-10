using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    // make settings save-able

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle cameraShakeToggle;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private bool ignore;
    private List<ResolutionEntry> resolutions;

    private void Awake()
    {
        resolutions = Screen.resolutions
            .Select(r => new ResolutionEntry { width = r.width, height = r.height })
            .Distinct()
            .OrderBy(r => r.height)
            .OrderBy(r => r.width)
            .ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions.Select(r => r.ToString()).ToList());
        resolutionDropdown.RefreshShownValue();

        LoadSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (ignore)
            return;

        var resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutionIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        if (ignore)
            return;

        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;
    }

    public void SetCameraShake(bool isShaking)
        => cameraShakeToggle.isOn = isShaking;

    public void SetVSync(bool value)
        => QualitySettings.vSyncCount = BoolToInt(value);

    public void SetVolume(float volume)
        => audioMixer.SetFloat("volume", volume);

    public void ChangeVolume(float volume)
        => volumeSlider.value += volume;

    public void SaveSettings()
    {
        //PlayerPrefs.SetInt("cameraShake", BoolToInt(cameraShakeToggle.isOn));
        PlayerPrefs.SetInt("vSync", BoolToInt(vSyncToggle.isOn));
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    private void LoadSettings()
    {
        // Resolution
        resolutionDropdown.value = resolutions.FindIndex(r => r.width == Screen.width && r.height == Screen.height);
        fullscreenToggle.isOn = Screen.fullScreen;
        SetVSync(IntToBool(PlayerPrefs.GetInt("vSync")));

        //SetCameraShake(IntToBool());
        ChangeVolume(PlayerPrefs.GetFloat("volume"));
    }

    private bool IntToBool(int value)
        => value > 0 ? true : false;

    private int BoolToInt(bool value)
        => value ? 1 : 0;

    struct ResolutionEntry
    {
        public int width;
        public int height;

        public override string ToString() => $"{width} x {height}";
    }
}
