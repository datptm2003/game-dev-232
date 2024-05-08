using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class TestRelay : MonoBehaviour
{
    public static TestRelay Instance { get; private set; }
	    
	private void Awake() {
        	Instance = this;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.C)) // Press 'C' to create relay
        {
            CreateRelay();
        }

        if (Input.GetKeyDown(KeyCode.J)) // Press 'J' to join relay (replace with your join code)
        {
            string joinCode = "YOUR_JOIN_CODE_HERE";
            JoinRelay(joinCode);
        }
    }


    private async void Start() {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () => {

        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
         
    }

    
    private async void CreateRelay() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();


        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    private async void JoinRelay(string joinCode) {
        try {
            Debug.Log("joining relay with" + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }
}


