using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Simple login handler: hook Unity UI InputFields and a Text for feedback.
// Set the expected username/password in the inspector or via code.
public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput; // Input field for username
    public TMP_InputField passwordInput; // Input field for password
    public TextMeshProUGUI feedbackText; // Text element for feedback

    public string expectedUsername = "Rui"; // Expected username
    public string expectedPassword = "123456"; // Expected password
    public string sceneToLoad = "CA4";   // Scene to load on successful login (set in inspector)

    // Called when the login button is clicked
    public void OnLoginButtonClicked()
    {
        string enteredUsername = usernameInput.text;
        string enteredPassword = passwordInput.text;

        // If credentials match, load the Test scene and feedback, if not, show error.
        if (enteredUsername == expectedUsername && enteredPassword == expectedPassword)
        {
            feedbackText.text = "Login successful!";
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            feedbackText.text = "Invalid username or password.";
        }
    }
}
