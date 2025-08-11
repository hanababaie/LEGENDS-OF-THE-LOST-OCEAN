using UnityEngine;
using UnityEngine.UI;

public class volume : MonoBehaviour
{
    [SerializeField] Slider volumeslider;
    [SerializeField] Image muteButtonIcon;
    [SerializeField] Sprite soundOnIcon;
    [SerializeField] Sprite soundOffIcon;

    public void Start()
    {
        if (!PlayerPrefs.HasKey("musicvalue"))
        {
            PlayerPrefs.SetFloat("musicvalue", 1);
        }
        else
        {
            load();
        }
    }
    public void changevolume()
    {
        AudioListener.volume = volumeslider.value;
        UpdateMuteIcon();
        save();
    }

    public void load()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicvalue");
        AudioListener.volume = savedVolume;
        volumeslider.value = savedVolume;
       
    }

    public void save()
    {
        PlayerPrefs.SetFloat("musicvalue", AudioListener.volume);
        PlayerPrefs.Save();
    }
    
     public void mute()
        {
            if (AudioListener.volume == 0)
            {
                AudioListener.volume = volumeslider.value > 0 ? volumeslider.value : 1f;
            }
            else
            {
                AudioListener.volume = 0f;
            }

            volumeslider.value = AudioListener.volume;
            UpdateMuteIcon();
            save();
        }

        private void UpdateMuteIcon()
        {
            muteButtonIcon.sprite = (AudioListener.volume == 0) ? soundOffIcon : soundOnIcon;
        }

}
