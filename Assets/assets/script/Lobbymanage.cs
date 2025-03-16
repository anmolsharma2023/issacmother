using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Collections.Generic;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using System.Linq;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;

public class Lobbymanage : MonoBehaviour
{
    public async void CreateRelay(int maxConnections = 3)
    {
        // Ensure Unity Services are initialized
        await UnityServices.InitializeAsync();

        // Authenticate user (anonymous authentication for simplicity)
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        try
        {
            // Create a relay allocation
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);


            // Get the secure DTLS endpoint
            var endpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

            var relayData = new RelayServerData(
                host: endpoint.Host,
                port: (ushort)endpoint.Port,
                allocationId: allocation.AllocationIdBytes,
                connectionData: allocation.ConnectionData,
                hostConnectionData: new byte[0], // Not needed for host
                key: allocation.Key,
                isSecure: true,
                isWebSocket: false
            );
        
            string joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
           Debug.Log("CCCOOODEEE "+joincode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Failed to create relay: {e.Message}");
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            // Ensure Unity Services are initialized
            await UnityServices.InitializeAsync();

            // Authenticate user (anonymous authentication for simplicity)
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            // Join the relay using the join code
         var joinAllocation=await RelayService.Instance.JoinAllocationAsync(joinCode);
            // Get matching endpoint type
            var endpoint = joinAllocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

            var relayData = new RelayServerData(
                host: endpoint.Host,
                port: (ushort)endpoint.Port,
                allocationId: joinAllocation.AllocationIdBytes,
                connectionData: joinAllocation.ConnectionData,
                hostConnectionData: joinAllocation.HostConnectionData,
                key: joinAllocation.Key,
                isSecure: true,
                isWebSocket: false
            );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);
            // Debug.Log($"Joined relay with allocation ID: {result.Allocation.AllocationId}");
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Failed to join relay: {e.Message}");
        }
    }
}
