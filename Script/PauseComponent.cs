using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class PauseComponent : MonoBehaviour
{
    [SerializeField]
    GameObject pauseScreen;
    [SerializeField]
    AudioMixer masterMixer;
    [SerializeField]
    GameObject musicSlider;
    [SerializeField]
    GameObject effectsSlider;

    void Awake()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(false);
        Invoke("DelayPauseAppear", 5);
        masterMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVolume"));
        masterMixer.SetFloat("effectsVol", PlayerPrefs.GetFloat("effectsVolume"));
        musicSlider.GetComponent<Slider>().value = GetMusicLevelFromMixer();
        effectsSlider.GetComponent<Slider>().value = GetEffectsLevelFromMixer();
    }

    float GetMusicLevelFromMixer()
    {
        float musicMixersValue;
        bool result = masterMixer.GetFloat("musicVol", out musicMixersValue);

        if (result)
        {
            return musicMixersValue;
        }
        else
        {
            return 0;
        }
    }

    float GetEffectsLevelFromMixer()
    {
        float musicMixersValue;
        bool result = masterMixer.GetFloat("effectsVol", out musicMixersValue);

        if (result)
        {
            return musicMixersValue;
        }
        else
        {
            return 0;
        }
    }

    public void SetMusicLevelFromSlider()
    {
        masterMixer.SetFloat("musicVol", musicSlider.GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.GetComponent<Slider>().value);
    }

    public void SetEffectsLevelFromSlider()
    {
        masterMixer.SetFloat("effectsVol", effectsSlider.GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("effectsVolume", effectsSlider.GetComponent<Slider>().value);
    }

    void SetPauseButtonActive(bool switchButton)
    {
        ColorBlock col = GetComponentInChildren<Toggle>().colors;

        if (switchButton == false)
        {
            col.normalColor = new Color32(0, 0, 0, 0);
            col.highlightedColor = new Color32(0, 0, 0, 0);
            col.pressedColor = new Color32(0, 0, 0, 0);
            col.disabledColor = new Color32(0, 0, 0, 0);
            GetComponentInChildren<Toggle>().interactable = false;
        }
        else
        {
            col.normalColor = new Color32(245, 245, 245, 255);
            col.highlightedColor = new Color32(245, 245, 245, 255);
            col.pressedColor = new Color32(200, 200, 200, 255);
            col.disabledColor = new Color32(200, 200, 200, 128);
            GetComponentInChildren<Toggle>().interactable = true;
        }
        GetComponentInChildren<Toggle>().colors = col;
        GetComponentInChildren<Toggle>().transform.GetChild(0).GetChild(0).gameObject.SetActive(switchButton);
    }

    void DelayPauseAppear()
    {
        SetPauseButtonActive(true);
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        SetPauseButtonActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(true);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        GameManager.Instance.GetComponent<ScoreManager>().ResetScore();
        GameManager.Instance.GetComponent<ScenesManager>().BeginGame(0);
    }
}