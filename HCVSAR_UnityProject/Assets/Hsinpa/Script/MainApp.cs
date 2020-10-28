using Expect.View;
using Hsinpa.Controller;
using Hsinpa.Socket;
using Hsinpa.View;
using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.StaticAsset;
using Hsinpa.Model;
using UnityEngine.Networking;

public class MainApp : Singleton<MainApp>
{
    protected MainApp() { } // guarantee this will be always a singleton only - can't use the constructor!

    private Subject subject;

    public SocketIOManager _socketManager;

    private Observer[] observers = new Observer[0];

    [SerializeField]
    private SimpleDatabase _database;
    public SimpleDatabase database => _database;

    private void Awake()
    {
        StartCoroutine(APIHttpRequest.LoadServerCSVLink());

        _socketManager = new SocketIOManager();
        APIHttpRequest.OnDynamicDomainIsGet += OnServerDomainIsReturn;
        subject = new Subject();

        RegisterAllController(subject);

        Init();
    }

    public void Notify(string entity, params object[] objects)
    {
        subject.notify(entity, objects);
    }

    public void Init() {
        LoginCtrl loginCtrl = GetObserver<LoginCtrl>();

        LoginModal loginModal = Modals.instance.GetModal<LoginModal>();
        loginCtrl.SetUp(loginModal, _socketManager);
        loginCtrl.ProcessLogin();
    }

    private void OnServerDomainIsReturn() {
        _socketManager.SetUpSocketIoManager();
        HostRoomCtrl hostRoomCtrl = GetObserver<HostRoomCtrl>();
        MonitorCtrl monitorCtrl = GetObserver<MonitorCtrl>();
        UserInfoCtrl userInfoCtrl = GetObserver<UserInfoCtrl>();
        ClassScoreCtrl classScoreCtrl = GetObserver<ClassScoreCtrl>();

        hostRoomCtrl.SetUp(Modals.instance.GetModal<HostRoomModal>(), _socketManager);
        monitorCtrl.SetUp(_socketManager);
        userInfoCtrl.SetUp(_socketManager, Modals.instance.GetModal<UserInfoModal>());
        classScoreCtrl.SetUp(Modals.instance.GetModal<ClassInfoModal>());
    }

    private void RegisterAllController(Subject p_subject)
    {
        Transform ctrlHolder = transform.Find("Controller");

        if (ctrlHolder == null) return;

        observers = transform.GetComponentsInChildren<Observer>();

        foreach (Observer observer in observers)
        {
            subject.addObserver(observer);
        }
    }


    public T GetObserver<T>() where T : Observer
    {
        foreach (Observer observer in observers)
        {
            if (observer.GetType() == typeof(T)) return (T)observer;
        }

        return default(T);
    }

    private void OnApplicationQuit()
    {
        if (_socketManager.IsConnected) {
            string leaveAPI = StringAsset.GetFullAPIUri(string.Format(Expect.StaticAsset.StringAsset.API.ManualDisconnect, _socketManager.socket.Id));
            StartCoroutine(APIHttpRequest.NativeCurl(leaveAPI, UnityWebRequest.kHttpVerbGET, null, null, null));
        }

        _socketManager.socket.Disconnect();
    }
}
