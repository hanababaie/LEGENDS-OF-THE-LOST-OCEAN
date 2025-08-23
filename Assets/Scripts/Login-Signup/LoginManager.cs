using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI messageText; // UI text for showing messages

    private string dataPath;

    void Start()
    {
        dataPath = Application.persistentDataPath + "/userdata.txt";
        messageText.text = ""; // clear message on start
    }

    public void SignUp()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Username and password cannot be empty.";
            messageText.color = Color.red;
            return;
        }

        // Save username and password (only one account per device)
        File.WriteAllText(dataPath, username + "," + password);
        Debug.Log("User signed up");

        messageText.text = "Sign up successful!";
        messageText.color = Color.green;

        SceneManager.LoadScene("Lobby");
    }

    public void SignIn()
    {
        if (!File.Exists(dataPath))
        {
            messageText.text = "No user data found. Please sign up first.";
            messageText.color = Color.red;
            return;
        }

        string[] data = File.ReadAllText(dataPath).Split(',');
        string savedUsername = data[0];
        string savedPassword = data[1];

        if (usernameInput.text == savedUsername && passwordInput.text == savedPassword)
        {
            Debug.Log("Login successful");
            messageText.text = "Login successful!";
            messageText.color = Color.green;

            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.Log("Wrong credentials");
            messageText.text = "Invalid username or password.";
            messageText.color = Color.red;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("mianmenu");
    }
}
