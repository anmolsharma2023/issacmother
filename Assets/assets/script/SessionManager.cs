using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}

public class SessionManager : Singleton<SessionManager> 
{
    ISession activeSession;
    public GameObject parentObject; // Assign in the Inspector
    public GameObject netmanage; // Assign in the Inspector
    ISession ActiveSession
    {
        get => activeSession;
        set
        {
            activeSession = value;
            Debug.Log($"Active session: {activeSession}");
        }
    }

    const string playerNamePropertyKey = "playerName";

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync(); // Initialize Unity Gaming Services SDKs.
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Anonymously authenticate the player
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
        
            var sessions = await QuerySessions();
            
            foreach (var session in sessions)
            {

                Debug.Log($"Session ID: {session.Id}, Join Code: x");
                
            }
            // Start a new session as a host
            //   StartSessionAsHost();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

  
   
    async UniTask<Dictionary<string, PlayerProperty>> GetPlayerProperties()
    {
        // Custom game-specific properties that apply to an individual player, ie: name, role, skill level, etc.
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        var playerNameProperty = new PlayerProperty(playerName, VisibilityPropertyOptions.Member);
        return new Dictionary<string, PlayerProperty> { { playerNamePropertyKey, playerNameProperty } };
    }

   public async void StartSessionAsHost()
    {
        var playerProperties = await GetPlayerProperties();
        
        var options = new SessionOptions
        {
            MaxPlayers = 3,
            IsLocked = false,
            IsPrivate = false,
            PlayerProperties = playerProperties
        }.WithRelayNetwork(); // or WithDistributedAuthorityNetwork() to use Distributed Authority instead of Relay

        ActiveSession = await MultiplayerService.Instance.CreateSessionAsync(options);
        NetworkManager.Singleton.StartHost();

        Debug.Log($"Session {ActiveSession.Id} created! Join code: {ActiveSession.Code}");
    }

    public async UniTaskVoid JoinSessionById(string sessionId)
    {
        ActiveSession = await MultiplayerService.Instance.JoinSessionByIdAsync(sessionId);
        Debug.Log($"Session {ActiveSession.Id} joined!");
        Debug.Log("bitch bich");
        NetworkManager.Singleton.StartClient();

    }

    async UniTaskVoid JoinSessionByCode(string sessionCode)
    {
        ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionCode);
        Debug.Log($"Session {ActiveSession.Id} joined!");
        NetworkManager.Singleton.StartClient();

    }

    async UniTaskVoid KickPlayer(string playerId)
    {
        if (!ActiveSession.IsHost) return;
        await ActiveSession.AsHost().RemovePlayerAsync(playerId);
    }

    public async UniTask<IList<ISessionInfo>> QuerySessions()
    {
        var sessionQueryOptions = new QuerySessionsOptions();
        QuerySessionsResults results = await MultiplayerService.Instance.QuerySessionsAsync(sessionQueryOptions);
        return results.Sessions;
    }

    async UniTaskVoid LeaveSession()
    {
        if (ActiveSession != null)
        {
            try
            {
                await ActiveSession.LeaveAsync();
            }
            catch
            {
                // Ignored as we are exiting the game
            }
            finally
            {
                ActiveSession = null;
            }
        }
    }
}