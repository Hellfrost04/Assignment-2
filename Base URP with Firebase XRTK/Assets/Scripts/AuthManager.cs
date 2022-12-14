using System.Collections;
using UnityEngine;
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using Firebase.Database;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class AuthManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference dbReference;
    

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;
    public GameObject MainMenu;
    public GameObject loginUI;
    public GameObject registerUI;
    
    
    public Slider clanChoice;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    private void Awake()
    {
        InitalizeFireBase();
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    //handle fire base in start
    void InitalizeFireBase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
    //clear the login fields
    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    //clear the register fields
    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }
    public async void SignUpNewUser()
    {
        string email = emailRegisterField.text.Trim();
        string password = passwordRegisterField.text.Trim();
        string repassword = passwordRegisterVerifyField.text.Trim();
        string username = usernameRegisterField.text.Trim();
        if( ValidateEmail(email) && ValidatePassword(password) && ValidateUserName(username))
        {
            
            if (password == repassword)
            {
                FirebaseUser newPlayer = await SignUpNewUserOnly(email, password);
                if (newPlayer != null)
                {
                    string clan = "blue";
                    float clanChoice = this.clanChoice.value;
                    if (clanChoice == 1)
                    {
                        clan = "yellow";
                    }
                    if (clanChoice == 2)
                    {
                        clan = "red";
                    }
                    await CreateNewDuckGamePlayer(newPlayer.UserId, username, username, newPlayer.Email, clan);
                    await UpdatePlayerDisplayName(username);//update users display name in auth service 
                }
            }
            else
            {
                warningRegisterText.text = "Error in Signing Up. Passwords are not the same";
                warningRegisterText.gameObject.SetActive(true);
            }
        }
        else
        {
            warningRegisterText.text = "Error in Signing Up. Invalid Email, Password or Username";
            warningRegisterText.gameObject.SetActive(true);
        }
       
        
    }
    public async Task<FirebaseUser> SignUpNewUserOnly(string email ,string password)
    {
        
        FirebaseUser newPlayer = null;
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {

            if (task.Exception != null)
                {
                    string errorMsg = this.HandleSignUpError(task);
                    warningRegisterText.text = errorMsg;
                    Debug.Log("Error in registering " + errorMsg);
                }
                //Debug.LogError("sorry, there was an error creating your new account,ERROR" + task.Exception);

            }else if (task.IsCompleted)
            {
                newPlayer = task.Result;
                ClearRegisterFields();
                registerUI.SetActive(false);
                loginUI.SetActive(true);
                Debug.LogFormat("New Player Details {0} {1}", newPlayer.UserId, newPlayer.Email);
            }

        });
        return newPlayer;
    }
    
    public async void SignInNewUser()
    {
        string email = emailLoginField.text.Trim();
        string password = passwordLoginField.text.Trim();
        if (ValidateEmail(email) && ValidatePassword(password))
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    string errorMsg = this.HandleSignInError(task);
                    warningLoginText.text = errorMsg;
                    Debug.Log("Error in signing in " + errorMsg);
                    warningLoginText.gameObject.SetActive(true);
                    //Debug.LogError("sorry, there was an error Signin your account,ERROR" + task.Exception);

                }
                else if (task.IsCompleted)
                {
                    FirebaseUser currentPlayer = task.Result;
                    
                    Debug.LogFormat("welcome to duck  game {0} {1}", currentPlayer.UserId, currentPlayer.Email);
                    SceneManager.LoadScene("MainMenu");
                }
            });
        }
        else
        {
            warningLoginText.text = "Error in Signing in . Invalid Email / Password";
            warningLoginText.gameObject.SetActive(true);
        }
        
    }
    public void SignOutUser()
    {
        if (auth.CurrentUser != null)
        {

            auth.SignOut();
            SceneManager.LoadScene("Register");
            Debug.Log("Signout successful");
        }
        else
        {
            Debug.Log("Signout not successful");
        }
    }
    /// <summary>
    /// simple client side email validation
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public bool ValidateEmail(string email)
    {
        bool isValid = false;
        //for all email have @ sign
       
        const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

        if(email != "" && Regex.IsMatch(email, pattern, options))
        {
            isValid = true;
        }
        return isValid;
    }
    /// <summary>
    /// Simple client side validation for password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool ValidatePassword(string password)
    {
        bool isValid = false;

        // length of the password at least 6 characters
        if(password != ""&& password.Length >= 6)
        {
            isValid = true;
        }
        return isValid;
    }
    /// <summary>
    /// simple client side validation for username
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public bool ValidateUserName(string userName)
    {
        bool isValid = false;

        
        if (userName != "" && userName.Length <= 15)
        {
            isValid = true;
        }
        return isValid;
    }
    //create new DUCKGAMEPlayer
    public async Task CreateNewDuckGamePlayer(string uuid, string displayName,
        string userName, string email, string clan)
    {
        DuckGamePlayer newPlayer = new DuckGamePlayer(displayName, userName, email, clan);
        Debug.LogFormat("Player details : {0}", newPlayer.PrintPlayer());

        //root/players/$uuid
        await dbReference.Child("players/" + uuid).SetRawJsonValueAsync(newPlayer.DuckGamePlayerToJson());

        await AddPlayerToClan(uuid, clan);

        //update auth player with new display name => tagging along the username
        await UpdatePlayerDisplayName(displayName);

    }
 
    public async Task AddPlayerToClan(string uuid, string clan)
    {
        Debug.LogFormat("AddPlayer {0} ToClan {1}: ", uuid, clan);
        //path: root/clans/$clan/$uuid
        await dbReference.Child("clans").Child(clan).Child(uuid).SetValueAsync(true);
    }
    public async Task UpdatePlayerDisplayName(string displayName)
    {
        if(auth.CurrentUser != null)
        {
            UserProfile profile = new UserProfile { 
                DisplayName = displayName };
            await auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => { 
            if(task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was cancelled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("User profile updated Successfully");
                Debug.LogFormat("checking current user display name form auth {0}", GetCurrentUserDisplayName());
            });
        }
    }
    public string GetCurrentUserDisplayName()
    {
        return auth.CurrentUser.DisplayName;
    }
    //Function for the forget password button
    public void ForgetPassword()
    {
        string email = emailLoginField.text.Trim();

        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Sorry, there was an sending a password reset, ERROR: " + task.Exception);

            }
            else if (task.IsCompleted)
            {
                Debug.Log("Forget Password email sent succefully...");
            }
        });
    }
    public FirebaseUser GetCurrentUser()
    {
        return auth.CurrentUser;
    }
    public string HandleSignUpError(Task<FirebaseUser> task)
    {
        string errorMsg = "";
        if(task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorcode = (AuthError)firebaseEx.ErrorCode;

            errorMsg = "Sign up Fail\n";
            switch (errorcode) 
            {
                case AuthError.EmailAlreadyInUse:
                    errorMsg += "Email already in use";
                    break;
                case AuthError.WeakPassword:
                    errorMsg += "Password is weak. Use at least 6 characters";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "Password is missing";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Invalid Email used";
                    break;
                default:
                    errorMsg += "Issure in authentication" + errorcode;
                    break;

            }
            Debug.Log("Error Message" + errorMsg);

        }
        return errorMsg;
    }
    public string HandleSignInError(Task<FirebaseUser> task)
    {
        string errorMsg = "";
        if (task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorcode = (AuthError)firebaseEx.ErrorCode;

            errorMsg = "Sign in Fail\n";
            switch (errorcode)
            {
                case AuthError.EmailAlreadyInUse:
                    errorMsg += "Email already in use";
                    break;
                case AuthError.WrongPassword:
                    errorMsg += "Password is wrong";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "Password is missing";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Invalid Email used";
                    break;
                case AuthError.UserNotFound:
                    errorMsg += "User not found";
                    break;
                default:
                    errorMsg += "Issure in authentication" + errorcode;
                    break;

            }
            Debug.Log("Error Message" + errorMsg);

        }
        return errorMsg;
    }
}
