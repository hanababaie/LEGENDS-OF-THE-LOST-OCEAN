using UnityEngine;
using UnityEngine.UI;

public class MuteButtonController : MonoBehaviour
{
    public Text buttonText;

    public void OnMuteButtonClick()
    {
        MusicManager.instance.ToggleMute();
        buttonText.text = MusicManager.instance.GetIsMuted() ? "Unmute" : "Mute";
    }
}