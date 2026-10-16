using System;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinMultiplayer : MonoBehaviour
{

    private async void Start()
    {
        // Initialize
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        // Connect
        QuickJoinOptions quickJoinOptions = new()
        {
            CreateSession = true
        };

        SessionOptions sessionOptions = new SessionOptions
        {
            MaxPlayers = 2
        }.WithRelayNetwork();

        await MultiplayerService.Instance.MatchmakeSessionAsync(quickJoinOptions, sessionOptions);
    }    

    // TODO: Handle return to main menu

    private void OnClientConnected(ulong clientId)
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (!manager.IsServer) return;
        if (manager.ConnectedClientsIds.Count < 2) return;

        manager.SceneManager.LoadScene("Multi", LoadSceneMode.Single);
    }
}