# Snapser Unity Demo - Single Player Game - Snap ship

## Install Unity

1. This demo project was built in 2022.3.8f. You could download the latest Unity version available or download the 2022.3.8f from the archives as mentioned below. You can run this example project in any 2021+ Unity version.
2. Unity Hub is the most convenient way to manage Unity installations. Typically, you should use the LTS version as they are the most stable. You can download Unity Hub for Windows/Mac from this link - [https://unity.com/unity-hub](https://unity.com/unity-hub)
3. Unity Hub allows you to download only the latest versions of Unity. You can download any version of Unity from the archives from here - [https://unity.com/releases/editor/archive](https://unity.com/releases/editor/archive)

## Setup Snapser

1. Go to [www.snapser.com](www.snapser.com) and login with your registered email address.
2. Create a new snapend (or you can choose to edit a snapend). On the following page, add the following snaps
    1. Authentication
    2. Leaderboard
    3. Profiles
    4. Statistics and segmentation
    5. Storage
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/createproject.gif)
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/addsnaps.gif)
3. Follow the rest of the steps to create your snapend.
4. Please note that it will take a few minutes to create your snapend.
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/creatingsnapendpopup.gif)
5. Once created, you will see your snapend ready in your dashboard.
6. From this dashboard, you can manage your snapend, download the sdk for your specific platform, access admin tools to update your snaps specific to your game, access api explorer etc.

## Setup Snapser Snaps

1. You can configure your individual snaps from the Admin Tools for your individual snapend.
    ![alt_text](https://github.com/snapser-engine/unity-demo-igdc/blob/main/Docs/Images/AdminToolsIntro.png)
2. For this demo, we are going to use anonymous login which is the easiest way to set up authentication for your game. You can choose to have alternate method such as email, facebook, google etc.
3. Just add an anon connector under ‘Add an connector’’
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/anonconnector.gif)
4. Similarly, configure profiles, statistics, storage and leaderboards as follows
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/profileconnector.gif)
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/statsconnector.gif)
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/storageconnector.gif)
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/leaderboardconnector.gif)

## Download Project

1. Clone or download this example project and open it in Unity.
2. After download the project will have a bunch of errors in the console. This is due to the missing Snapser sdk and its dependencies.

## Install Snapser for Unity

1. In Unity, for the Snapser SDK to work, we need to install a few dependencies. A convenient way to install the dependencies is to use NuGetForUnity. This is a convenient nuget interface from within Unity but you can choose to install these dependencies from the Terminal if you wish. Follow the instructions to install NuGetForUnity -
    1. [https://github.com/GlitchEnzo/NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)
    2. You may need to restart Unity to access the Nuget via the Unity Toolbar as follows -
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/opennuget.gif)
2. From **Manage NuGet Packages**, Install the following three dependencies -
    1. Newtonsoft.Json
    2. Polly
    3. Microsoft.AspNetCore.Mvc.DataAnnotations
    4. Note: These packages will be installed under **Assets/Packages** in your Unity Project. Also sometimes NuGet installs a different version of DataAnnotations. If it does that please uninstall it from the UI.
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/adddependenciesnuget.gif)
4. Download your Snapser SDK from the Snapser Portal. Move the downloaded SDK into the **Assets/Packages** folder.
5. That’s it! You can use the Snapser SDK to make calls to your snaps. Under {Snapser SDK}/SDK/Snapser/Api, You should have a class for each of your snaps.

## Usage

* To understand the usage, you should look at SnapserManager. This class acts as the link between the game logic and the Snapser sdk.
* For example, the following function makes an anonymous login authentication call to the Snapser’s authentication service.
```

public async void AuthenticationAnonLoginAsync(Action&lt;SnapserServiceResponse> callback = null)

{

   if (ClusterURL.IsEmptyOrNull())

   {

       Debug.LogError("No cluster url available to start authentication from user/auth services.");

       return;

   }

   if (_authService == null)

   {

       Debug.LogError("Auth service is not instantiated. This indicates there is a flow issue between cluster creation and the authentication call. Auth call can't be initiated.");

       return;

   }

   AuthAnonLoginResponse response;

   SnapserServiceResponse authResponse;

   try

   {

       if (UserName.IsEmptyOrNull())

       {

           UserName = GameConstants.DefaultPlayerName + DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");

       }

       response = await _authService.AnonLoginAsync(new AuthAnonLoginRequest(true, UserName));

   }

   catch (Exception e)

   {

       Console.WriteLine(e);

       authResponse = new SnapserServiceResponse(false, e.Message, e);

       callback?.Invoke(authResponse);

       Debug.Log("Authentication Call Complete - " + authResponse.Success + " - " + authResponse.ResponseMessage);

       return;

   }

   if (response != null)

   {

       UserId = response.User.Id;

       SessionToken = response.User.SessionToken;

       authResponse = new SnapserServiceResponse(true, "Ok", response);

       callback?.Invoke(authResponse);

       Debug.Log("Authentication Call Complete - " + authResponse.Success + " - " + authResponse.ResponseMessage + ". UserID - " + response.User.Id + ". SessionToken - " + response.User.SessionToken);

       IsAuthenticated = true;

       InstantiateRequiredSnapserServices();

       OnAuthenticationSuccessful?.Invoke();

   }

}
```

## Demo

1. Once all the dependencies are installed, run the Main scene which can be found in the Project window under Assets/Scenes. Hit the play button to start the game.
2. After you hit the “Login and Power up Snapser” button, you’ll see the UI update as such -
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/snapshipdemologin.gif)
3. Each of the UI has its own game object with specific classes
    ![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Images/Screenshot%202023-09-14%20175827.png)
4. You can change the cluster from a default one to your own in the Game Manager game object instance in the scene hierarchy.
5. This example project will help you understand how to communicate with Snapser’s authentication, profiles, storage, stats and leaderboard services. You can add/remove snaps as you please and add your own handlers in SnapserManager or any other sensible class.

If you have questions or require support please let us know and we will do our best to assist you. We’d love to hear more about your project so please let us know.
