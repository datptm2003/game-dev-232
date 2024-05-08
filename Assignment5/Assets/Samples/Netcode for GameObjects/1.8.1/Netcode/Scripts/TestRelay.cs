using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using System.Threading.Tasks;

public class TestRelay : MonoBehaviour
{
    public static TestRelay Instance { get; private set; }
	    
	private void Awake() {
        	Instance = this;
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


        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    private async void JoinRelay(string joinCode) {
        try {
            Debug.Log("joining relay with" + joinCode);
            await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }
}


