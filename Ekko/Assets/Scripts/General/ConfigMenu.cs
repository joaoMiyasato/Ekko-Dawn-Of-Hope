using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    public GameObject bar;

    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

#region volume
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
#endregion

#region gráficos
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
#endregion

#region janela
    private bool isFullScreen = true;
    public void SetWindow(int setScreenMode)
    {
        if(setScreenMode == 0)
        {
            isFullScreen = true;
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if(setScreenMode == 1)
        {
            isFullScreen = false;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if(setScreenMode == 2)
        {
            isFullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
#endregion

#region resolução
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
    }
#endregion


}
