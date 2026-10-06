using System.Collections.Generic;
using UnityEngine;
using gm.api.domain;
using BestHTTP.SignalRCore;
using System;
using BestHTTP.SignalRCore.Encoders;
using gm.api.domain.Models.GameDetail;
using System.Threading.Tasks;
using System.Collections;

public class GameSocketManager : MonoBehaviour
{
    private HubConnection _hubConnection;
    public static GameSocketManager Instance;

    public static string UserId_;
    public static string UserName_;
    public static GmWallet wallet_;
    public List<GmGame> games;
    public List<string> notifications;
    public List<string> announcements;
    internal float maxPercentage;

    public bool IsGamelistLoaded = false;
    public bool IsAnnounced = false;



    internal bool HighWin_forced = false;
    internal int spins_counter;

    //internal GmKhelChance General_chance;

    public delegate void Authenticate(GmWebSocketResponse<GmLoginResponse> response);
    public static event Authenticate OnAuthenticated;

    public delegate void GetWallet(GmWebSocketResponse<GmWallet> response);
    public static event GetWallet OnGetWallet;

    public delegate void GetGameList(GmWebSocketResponse<List<GmGame>> response);
    public static event GetGameList OnGetGameList;

    public delegate void GetGameDetail(GmWebSocketResponse<GmGame> response);
    public static event GetGameDetail OnGetGameDetail;

    public delegate void UnAuthorized(GmWebSocketResponse<string> response);
    public static event UnAuthorized OnUnAuthorized;

    public delegate void Error(GmWebSocketResponse<string> response);
    public static event Error OnError;

    public GmLoginResponse auth;
    public GmWallet wallet;
    public bool IsServerConnected = false;
    public string HUB_URL = "https://gm-api.ahaorders.com";

    // Start is called before the first frame update
    public void Connect()
    {
        HubOptions options = new HubOptions();
        _hubConnection = new HubConnection(new Uri($"{HUB_URL}/user-control"), new JsonProtocol(new LitJsonEncoder()), options);

        _hubConnection.On<GmWebSocketResponse<GmLoginResponse>>(nameof(GmRequestType.AUTHENTICATE), res =>
        {
            // Debug.Log("Auth Received");
            auth = res.Data;
            OnAuthenticated(res);
        });
        _hubConnection.On<GmWebSocketResponse<GmWallet>>(nameof(GmRequestType.WALLET), res =>
        {
            Debug.Log("Wallet Received");
            OnGetWallet(res);
        });
        _hubConnection.On<GmWebSocketResponse<List<GmGame>>>(nameof(GmRequestType.GAME_LIST), res =>
        {
            Debug.Log("Game List Received");
            OnGetGameList(res);
        });
        _hubConnection.On<GmWebSocketResponse<GmGame>>(nameof(GmRequestType.GAME_DETAIL), res =>
        {
            Debug.Log("Game Detail Received");
            OnGetGameDetail(res);
        });
        _hubConnection.On<GmWebSocketResponse<string>>(nameof(GmRequestType.UNAUTHORIZED), res =>
        {
            Debug.Log("Unauthorized Received");
            OnUnAuthorized(res);
        });

        _hubConnection.On<GmWebSocketResponse<string>>(nameof(GmRequestType.ERROR), res =>
        {
            Debug.Log("Error Received");
            OnError(res);
        });

        _hubConnection.OnConnected += Hub_OnConnected;
        _hubConnection.OnError += Hub_OnError;
        _hubConnection.OnClosed += Hub_OnClosed;

        _hubConnection.StartConnect();
    }

    private void Hub_OnClosed(HubConnection obj)
    {
        Debug.Log("Connection Closed");
        IsServerConnected = false;
    }

    private void Hub_OnError(HubConnection arg1, string arg2)
    {
        IsServerConnected = false;
        Debug.LogError(arg1);
    }

    private void Hub_OnConnected(HubConnection obj)
    {
        IsServerConnected = true;
        Debug.Log("Hub Connected");
    }

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Connect();


    }

    #region Actions

    public void LoginAsync(string username, string password)
    {
        var request = new GmWebSocketRequest<GmLoginRequest>
        {
            Data = new GmLoginRequest
            {
                UserName = username,
                Password = password
            }
        };
        Debug.Log("Login Invoke");
        _hubConnection.InvokeAsync<GmWebSocketResponse<GmLoginResponse>>(nameof(GmRequestType.AUTHENTICATE), request);
        Debug.Log("Login Invoked");
    }

    public void InvokeGetWalletAsync()
    {
        var request = new GmWebSocketRequest<string>
        {
            Token = this.auth.JwtToken,
            Data = this.auth.Id
        };
        Debug.Log("Get wallet invoke");
        _hubConnection.InvokeAsync<GmWebSocketResponse<GmWallet>>(nameof(GmRequestType.WALLET), request);
    }

    public void InvokeGetGameListAsync()
    {
        var request = new GmWebSocketRequest<string>
        {
            Token = this.auth.JwtToken,
            Data = this.auth.Id
        };
        Debug.Log("Gamelist Invoke");

        _hubConnection.InvokeAsync<GmWebSocketResponse<GetGameList>>(nameof(GmRequestType.GAME_LIST), request);
    }

    public void InvokeGetGameDetailAsync(string gameName, string gameId)
    {
        var request = new GmWebSocketRequest<GmGameDetailRequest>
        {
            Token = this.auth.JwtToken,
            Data = new GmGameDetailRequest
            {
                GameId = gameId,
                GameName = gameName
            }
        };
        _hubConnection.InvokeAsync<GmWebSocketResponse<GmGameDetailRequest>>(nameof(GmRequestType.GAME_DETAIL), request);
    }
    #endregion




}
