using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject searchGameButton;
    public GameObject cancelButton;

    private int randomRoomName;

    private void Awake()
    {
        lobby = this; // Singleton, lives within main menu
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Master photon server
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        searchGameButton.SetActive(true);
    }
    public void OnsearchGameButtonnClicked()
    {
        Debug.Log("Search Game Clicked");
        searchGameButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); // Try to join a random room
    }
    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Search Game Clicked");
        cancelButton.SetActive(false);
        searchGameButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        CreateRoom();
    }
    void CreateRoom()
    {
        randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, 
            IsOpen = true, 
            MaxPlayers = (byte)MultiplayerSettings.mpSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOptions);
        Debug.Log("Created new room: " + randomRoomName);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }
}
