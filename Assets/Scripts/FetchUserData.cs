using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;

// using Newtonsoft.Json;


[System.Serializable]
public class UserData
{
    public string username;
    public int coins;
    public int maxDistTravelled;
    public int lastOneDistTravelled;
    public int lastTwoDistTravelled;
}

[System.Serializable]
public class PlayerData
{
    public int coins;
    public int maxDistTravelled;
    public int lastOneDistTravelled;
    public int lastTwoDistTravelled;
}

[System.Serializable]
public class UserRankData
{
    public int rank;
    public int distance;
    public int coins;
}

public class FetchUserData : MonoBehaviour
{
    private const string baseURL = "http://localhost:3000/api/user/";

    // public string username;

    public void SaveUserInfo(string uName, int coins, int maxDistTravelled, int lastOneDistTravelled, int lastTwoDistTravelled)
    {
        // Replace these values with the updated score data
        StartCoroutine(SaveUserData(uName, coins, maxDistTravelled, lastOneDistTravelled, lastTwoDistTravelled));
    }

    private IEnumerator SaveUserData(string uName, int coins, int maxDistTravelled, int lastOneDistTravelled, int lastTwoDistTravelled)
    {
        string url = baseURL + uName + "/save";
        
        PlayerData playerData = new PlayerData
        {
            coins = coins,
            maxDistTravelled = maxDistTravelled,
            lastOneDistTravelled = lastOneDistTravelled,
            lastTwoDistTravelled = lastTwoDistTravelled
        };
        
        // Create a JSON object to send in the request body
        string json = JsonUtility.ToJson(playerData);

        Debug.Log("NJ: json : " + json + " coins : " + coins);
        using (UnityWebRequest request = UnityWebRequest.Post(url, json))
        {
            // Set the content type header to JSON
            request.SetRequestHeader("Content-Type", "application/json");

            // Set the request body
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error saving user data: " + request.error);
            }
            else
            {
                Debug.Log("Score updated successfully!");
            }
        }
    }
    
    public void FetchUSerRank(string userName, Action<UserRankData> function) {
        userDataActionCallback = function;
        StartCoroutine(FetchUserRank(userName));
    }

    public Action<UserRankData> userDataActionCallback;
    
    private IEnumerator FetchUserRank(string userName)
    {
        string url = baseURL + userName + "/rank";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching user rank: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                UserRankData rankData = JsonConvert.DeserializeObject<UserRankData>(json);
                userDataActionCallback(rankData);
                // Use rankData.rank to display the user's rank in Unity
                Debug.Log("User " + userName + " Rank: " + rankData.rank + " coins : " + rankData.coins);
            }
        }
    }
    
    public void updateUserScoreData(string userName, int userScore) {
        StartCoroutine(UpdateTraveledDistance(userName, userScore));
    }

    private IEnumerator UpdateTraveledDistance(string userName, int userScore)
    {
        string url = baseURL + userName + "/update-distance";

        // Create a JSON object to send in the request body
        string json = JsonUtility.ToJson(new
        {
            userScore
        });

        using (UnityWebRequest request = UnityWebRequest.Post(url, json))
        {
            // Set the content type header to JSON
            request.SetRequestHeader("Content-Type", "application/json");

            // Set the request body
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error updating traveled distance: " + request.error);
            }
            else
            {
                Debug.Log("Traveled distances updated successfully!");
            }
        }
    }
    
    public void FetchLeaderboardData()
    {
        StartCoroutine(FetchLeaderboard());
    }

    private IEnumerator FetchLeaderboard()
    {
        string baseURL = "http://localhost:3000/api/leaderboard";
        using (UnityWebRequest request = UnityWebRequest.Get(baseURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching leaderboard: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                UserData[] userDataListWrapper = JsonConvert.DeserializeObject<UserData[]>(json);

                // Access the list of UserData objects.
                foreach (UserData userData in userDataListWrapper)
                {
                    Debug.Log("Username: " + userData.username + "Coins: " + userData.coins + "Max Distance Travelled: " + userData.maxDistTravelled);
                    // Add more properties as needed...
                }
            }
        }
    }
}
[System.Serializable]
public class UserDataList
{
    public List<UserData> userDataList;
}

