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
6. Click on the Snapend widget, to go to your home page. **IMPORTANT**: Here copy the **Snapend ID** from the top. You will need that in the Step - `Updating the Snapend ID in the SDK code`.
7. From this dashboard, you can manage your snapend, download the sdk for your specific platform, access admin tools to update your snaps specific to your game, access api explorer etc.

## Setup Snapser Snaps
You can configure your individual snaps from the Admin Tools for your individual snapend.
![alt_text](https://github.com/snapser-engine/unity-demo-igdc/blob/main/Docs/Images/AdminToolsIntro.png)

### Automated Setup
You can use Snapser's infrastructure and configuration as code, to directly import a file and create
a Snapend with the infra and configuration you need for this game.

- This project has a manifest file called `snapser-f7auppyi-manifest.json`
- This file contains the infra architecture and the configuration data already.
- Go to your Game home page and click on the **Create Snapend** button. Development or Staging, either is fine.
- Give your Snapend a name and hit Continue.
- In the next step, instead of selecting Snaps, click the blue button and select **Import**.
- Now, select this manifest file and Snapser will automatically load everything in.
- Keep hitting Continue till you reach the last step and then hit **Snap it**.
- Your Snapend will be up and ready to be used in this game.

### Manual Setup
#### Setup Auth
- For this demo, we are going to use anonymous login which is the easiest way to set up authentication for your game. You can choose to have alternate method such as email, facebook, google etc.
- Go to the **Snapend Configuration** tool, and select **Auth**
- Click on the **Connector** tool, and select Anon from the drop down.
- Hit Save and your Anon login will be active.
![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/anonconnector.gif)

#### Setup Profile
- Go to the **Snapend Configuration** tool, and select **Profile**
- Click on the **Attributes** tool and add a new attribute called **username**.
- Hit Save.
![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/profileconnector.gif)

#### Setup Statistics
- Go to the **Snapend Configuration** tool, and select **Statistics and Segmentation**
- Click on the **Statistics** tool and add a new stat called **distance_travelled**.
- Hit Save.
![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/statsconnector.gif)

#### Setup Storage Blob
- Go to the **Snapend Configuration** tool, and select **Storage**
- Click on the **Blobs** tool and add a new blob called **color**.
- Hit Save.
![alt_text](https://github.com/snapser-community/snapser-demo-unity-sp/blob/main/Docs/Gifs/storageconnector.gif)

#### Setup Leaderboard
- Go to the **Snapend Configuration** tool, and select **Leaderboards**
- Click on the **Leaderboards** tool and add a new leaderboard called **max_distance**.
- Hit Save.
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

## Updating the Snapend ID in the SDK code
**IMPORTANT**: This project already has a SDK added for your convenience. You can either replace
the code by downloading the SDK from the Snapend you just spun up. Or then just do a global search
for the following line and replace `f7auppyi` with your Snapend Id.
```
https://gateway.snapser.com/f7auppyi
```
There should be two files where you need to swap the value
- Configuration.cs
- GameConstants.cs

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
