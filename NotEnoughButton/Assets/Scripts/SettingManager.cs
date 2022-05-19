using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    private bool isOpen;

    //Volume
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    //resolution
    public List<Resolution> resolutions = new List<Resolution>();
    public Dropdown resolutionDropdown;
    public List<Vector2> resolutionOptions = new List<Vector2>();


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        isOpen = false;

        //volume
        volumeSlider.value = -10f;

        //resolution set
        Resolution[] tmpResolutions = Screen.resolutions;//save all resolutions
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();//save strings for dropdown
        int currentResolutionIndex = 0;
        for (int i = 0; i < tmpResolutions.Length; i++)
        {
            Vector2 check = new Vector2(tmpResolutions[i].width, tmpResolutions[i].height);
            //Debug.Log(check);
            if (resolutionOptions.Contains(check))
            {
                resolutions.Add(tmpResolutions[i]);
                string option = tmpResolutions[i].width + " x " + tmpResolutions[i].height;
                options.Add(option);
                if (tmpResolutions[i].width == Screen.currentResolution.width &&
                    tmpResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = resolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public void OpenSetting()
    {
        GameManager.instance.stopEverything = true;
        transform.GetChild(0).gameObject.SetActive(true);
        isOpen = true;
    }

    public void CloseSetting()
    {
        GameManager.instance.stopEverything = false;
        transform.GetChild(0).gameObject.SetActive(false);
        isOpen = false;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Debug.Log(resolution);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetGraphic(int graphicLevel)
    {
        QualitySettings.SetQualityLevel(graphicLevel);
    }

}
