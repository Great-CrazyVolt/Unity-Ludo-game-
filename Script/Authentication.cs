using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Firebase.Database;

public class UserData
{
    public string name;
    public string email;
    public string password;

    public UserData(string name, string email, string password)
    {
        this.name = name;
        this.email = email;
        this.password = password;
    }

    public UserData(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}

public class Authentication : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField username;
    public InputField email;
    public InputField password;

    public Button loginButton;

    // MainMenu and AuthenticationForms
    public GameObject MainMenu;
    public GameObject AuthenticationForm;

    [SerializeField] private Text emailError;
    private string emailPattern;

    private DatabaseReference dbReference;
    // private int userID;

    private bool loginSuccess = false;
    private bool loginFailed = false;
    void Start()
    {
        // loginButton.onClick.AddListener(RegisterUser);

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        // Check if login is successful
        if (loginSuccess)
        {
            // Show the main menu and hide the authentication form
            MainMenu.SetActive(true);
            AuthenticationForm.SetActive(false);
            Debug.Log("Successfully logged In");
            // Reset the loginSuccess flag
            loginSuccess = false;
        }

        // Check if login failed
        if (loginFailed)
        {
            // Handle the case where the provided user input is not found in the database
            Debug.Log("Invalid Credentials");
            // Reset the loginFailed flag
            loginFailed = false;
        }
    }

    public void RegisterUser()
    {
        // userID = Random.Range(1, 100);
        // userID = SystemInfo.deviceUniqueIdentifier;
        UserData userData = new UserData(username.text, email.text, password.text);

        string json = JsonUtility.ToJson(userData);

        dbReference.Child("users").Push().SetRawJsonValueAsync(json);
        // dbReference.Child("users").Child(email.text.ToString()).SetRawJsonValueAsync(json);
        // dbReference.Child("status").Child(username.text).SetRawJsonValueAsync(json);
        Debug.Log("Added successfully");

        MainMenu.SetActive(true);
        AuthenticationForm.SetActive(false);
    }

    public void LoginUser()
    {
        DatabaseReference userRef = dbReference.Child("users");

        Query query = userRef.OrderByChild("email").EqualTo(email.text);

        query.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                loginSuccess = true;
            }
            else
            {
                loginFailed = true;
            }
            // if (snapshot.Exists)
            // {

            //     // foreach (DataSnapshot childSnapshot in snapshot.Children)
            //     // {
            //     //     string userKey = childSnapshot.Key;
            //     //     string email = childSnapshot.Child("email").Value.ToString();
            //     //     string password = childSnapshot.Child("password").Value.ToString();

            //     //     Debug.Log("userKey: " + userKey);
            //     //     Debug.Log("email: " + email);
            //     //     Debug.Log("password: " + password);
            //     // }

            //     MainMenu.gameObject.SetActive(true);
            //     AuthenticationForm.gameObject.SetActive(false);

            //     Debug.LogFormat("Successfully logged In");

            // }
            // else
            // {
            //     Debug.LogFormat("Invalid Credentials");
            //     // Debug.Log();
            // }
        });

        // Debug.Log("email: " + email.text);

        // var userData = dbReference.OrderByChild("email").EqualTo(email.text).GetValueAsync();

        // yield return new WaitUntil(predicate: () => userData.IsCompleted);

        // DataSnapshot snapshot = userData.Result;

        // if (snapshot != null)
        // {
        //     string jsonValue = snapshot.GetRawJsonValue();
        //     if (jsonValue == "{}") // Check if the JSON is empty
        //     {
        //         Debug.Log("The raw JSON is empty.");
        //     }
        //     else
        //     {
        //         // Deserialize the JSON data into UserData class
        //         string json = snapshot.GetRawJsonValue();


        //         // Do something with the userData (e.g., display it or store it in a list)
        //         // Debug.Log("User ID: " + snapshot.Key);
        //         // Debug.Log("Username: " + useValues.name);
        //         // Debug.Log("Email: " + useValues.email);
        //         // Debug.Log("Password: " + useValues.password);

        //         Debug.Log(jsonValue);

        //         // Do something with the userData (e.g., authenticate the user)
        //     }
        // }
        // else
        // {
        //     Debug.LogWarning("Snapshot is null. Data not found for the given email.");
        // }

        // if (userData.Exception != null) //basic error checking, unsure if it actually does anything here of use
        // {
        //     Debug.Log("Error at CheckIfUsernameExists");
        // }
        // else
        // {
        //     Debug.Log("Successfully logged In");

        //     MainMenu.gameObject.SetActive(true);
        //     AuthenticationForm.gameObject.SetActive(false);
        // }

    }

    // public void checkUserCredentials()
    // {
    //     StartCoroutine(loginUser());
    // }

    public void checkEmailValidation()
    {
        emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        bool isEmailMatched = Regex.IsMatch(email.text, emailPattern);

        if (!isEmailMatched)
        {
            emailError.gameObject.SetActive(true);
            emailError.color = Color.red;
            emailError.text = "Invalid Email";

            loginButton.gameObject.SetActive(false);
        }
        else
        {
            emailError.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(true);

        }

        Debug.Log("username: " + username.text);
    }

    public void GetInputFromUser()
    {
        Debug.Log("UserInput:" + username.text + " " + password.text);
    }
}
