using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    private string dataPath;

    void Start()
    {
        dataPath = Application.persistentDataPath + "/userdata.txt";
    }

    public void SignUp()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        File.WriteAllText(dataPath, username + "," + password);
        Debug.Log("User signed up");
        SceneManager.LoadScene("Lobby");
    }

    public void SignIn()
    {
        if (!File.Exists(dataPath))
        {
            Debug.Log("No user data found");
            return;
        }

        string[] data = File.ReadAllText(dataPath).Split(',');
        if (usernameInput.text == data[0] && passwordInput.text == data[1])
        {
            Debug.Log("Login successful");
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.Log("Wrong credentials");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("mianmenu");
    }
}