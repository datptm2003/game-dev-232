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
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class TestRelay : MonoBehaviour
{

    public static TestRelay Instance { get; private set; }
    public InputField JoinCodeInput;
	

	private void Awake() {
        	Instance = this;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateRelay();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) // Press 'Enter' to join relay (replace with your join code)
        {
            if (String.IsNullOrEmpty(JoinCodeInput.GetComponent<Text>().text))
            {
                Debug.LogError("Please input a join code.");
                return;
            }
            try
                {
                    JoinRelay(JoinCodeInput.GetComponent<Text>().text);
                }
            catch (RelayServiceException ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
            }

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
            // InventorySystem.Instance.currentJoinCode.GetComponent<Text>().text = joinCode;
            CraftingController.Instance.currentJoinCode.GetComponent<Text>().text = "Room Code: " + joinCode;

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();


        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with code: " + joinCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to join relay: " + e.Message);
        }
    }
}


