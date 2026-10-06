using System.Collections.Generic;
using UnityEngine;
using gm.api.domain;
using BestHTTP.SignalRCore;
using System;
using BestHTTP.SignalRCore.Encoders;
using gm.api.domain.Models.GameDetail;
using gm.api.domain.Models;
using gm.api.domain.Models.GameControl;

public  class GamePlaySocketManager : MonoBehaviour
{
    private HubConnection _hubConnection;
    public static GamePlaySocketManager Instance;
    public bool hubConnected;
    public static string gameId;
    public static string gameName;
    public static string AssetPath;
    public static string SceneName;

    
    #region Hub

    public void Connect()
    {
        return; //Kaleemchange
       
        HubOptions options = new HubOptions();
        _hubConnection = new HubConnection(new Uri($"{GameSocketManager.Instance.HUB_URL}/game-control"), new JsonProtocol(new LitJsonEncoder()), options);
        _hubConnection.OnConnected += Hub_OnConnected;
        _hubConnection.OnError += Hub_OnError;
        _hubConnection.OnClosed += Hub_OnClosed;

        _hubConnection.On<GmWebSocketResponse<GmPlayModel>>(nameof(GmRequestType.PLAY_GAME), res => {
          
            OnPlayGame(res);
        });

        _hubConnection.On<GmWebSocketResponse<GmPlayModel>>(nameof(GmRequestType.COMPLETE_GAME), res => {
            OnCompleteGame(res);
        });


        _hubConnection.StartConnect();
    }

    #region Game Actions

    public void InvokePlayGame(string gameId, string gameName,double playAmount)
    {
        return; //Kaleemchange
        var request = new GmWebSocketRequest<GmPlayGameRequest>
        {
            Token = GameSocketManager.Instance.auth.JwtToken,
            Data = new GmPlayGameRequest { 
             GameId = gameId,
             GameName = gameName,
             PlayAmount = playAmount,
             UserId = GameSocketManager.Instance.auth.Id,
             Username = GameSocketManager.Instance.auth.Username
            }
        };
        Debug.Log("Play Game Invoked");
        _hubConnection.InvokeAsync<GmWebSocketResponse<GmPlayModel>>(nameof(GmRequestType.PLAY_GAME), request);
    }

    public void InvokeCompleteGame(string gameId, string gameName, string securityToken, double winAmount)
    {
        var request = new GmWebSocketRequest<GmCompleteGameRequest>
        {
            Token = GameSocketManager.Instance.auth.JwtToken,
            Data = new GmCompleteGameRequest
            {
                GameId = gameId,
                GameName = gameName,
                UserId = GameSocketManager.Instance.auth.Id,
                SecurityToken = securityToken,
                WinAmount = winAmount
            }
        };
        Debug.Log("Complete Game Invoked");
        _hubConnection.InvokeAsync<GmWebSocketResponse<GmPlayModel>>(nameof(GmRequestType.COMPLETE_GAME), request);
    }


    #endregion

    #region Server/Hub Actions

    #endregion

    public virtual void OnPlayGame(GmWebSocketResponse<GmPlayModel> res) { 
    
    }

    public virtual void OnCompleteGame(GmWebSocketResponse<GmPlayModel> res)
    {

    }
    public virtual void Hub_OnError(HubConnection arg1, string arg2) {
        hubConnected = false;
        Debug.LogError(arg2);
    }
    public virtual void Hub_OnClosed(HubConnection obj) {
        hubConnected = false;
        Debug.Log("Hub Closed");
    }
    public virtual void Hub_OnConnected(HubConnection obj) {
        hubConnected = true;
        Debug.Log("Hub Connected");
    }
    #endregion
}
