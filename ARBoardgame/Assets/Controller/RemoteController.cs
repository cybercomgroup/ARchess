using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

#if UNITY_ANDROID
// aaaaaa
#endif

public class RemoteController : WifiDirectBase {
    private volatile bool host;
    private volatile bool client;
    private volatile bool _isReady;
    private volatile int isBroadcasting;
    private volatile bool isConnected;

    void Start()
    {
        host = false;
        client = false;
        _isReady = false;
        isBroadcasting = 0;
        isConnected = false;
        base.initialize(this.gameObject.name); // init WiFi Direct
    }
    
    public void HostServer() // safe to call any number of times
    {
        if (client) return; // silent error, is a client!
        host = true;
        if (_isReady) HostBroadcast();
    }
    
    public void JoinServer() // safe to call any number of times
    {
        if (host) return; // silent error, is hosting!
        client = true;
        if (_isReady) JoinBroadcast();
    }

    //when the WifiDirect services is connected to the phone, begin broadcasting and discovering services
    public override void onServiceConnected()
    {
        _isReady = true;
        if (host) HostBroadcast();
        else if (client) JoinBroadcast();

    }

    private void HostBroadcast()
    {
        if (Interlocked.CompareExchange(ref isBroadcasting, 1, 0) == 0)
        {
            Dictionary<string, string> record = new Dictionary<string, string>();
            record.Add("demo", "unity");
            base.broadcastService("host", record);
            base.discoverServices();
        }
    }

    private void JoinBroadcast()
    {
        if (Interlocked.Exchange(ref isBroadcasting, 1) == 0)
        {
            Dictionary<string, string> record = new Dictionary<string, string>();
            record.Add("demo", "unity");
            base.broadcastService("join", record);
            base.discoverServices();
        }
    }

    //On finding a service, create a button with that service's address
    public override void onServiceFound(string addr)
    {
        // ALLOW USER TO SEE POTENTIAL CONNECTION
        // CONNECT BY: this.makeConnection(addr); // addr is address string
    }
    //When the button is clicked, connect to the service at its address
    private void makeConnection(string addr)
    {
        base.connectToService(addr);
    }

    public override void onConnect()
    {
        isConnected = true;
        // ANY CALLS TO base.sendMessage(arg); FROM NOW ON SENDS TO OTHER PLAYER.
    }
    
    public override void onMessage(string message)
    {
        // THIS IS THE MESSAGE WE RECEIVED

        // PARSE MESSAGE AND SEND AS EVENT.
    }


    public override void onServiceDisconnected()
    {
       // base.terminate();
       // Application.Quit();
    }
    
}
