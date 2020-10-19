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
        _socketManager = new SocketIOManager();
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
        HostRoomCtrl hostRoomCtrl = GetObserver<HostRoomCtrl>();
        MonitorCtrl monitorCtrl = GetObserver<MonitorCtrl>();
        UserInfoCtrl userInfoCtrl = GetObserver<UserInfoCtrl>();
        ClassScoreCtrl classScoreCtrl = GetObserver<ClassScoreCtrl>();

        LoginModal loginModal = Modals.instance.GetModal<LoginModal>();
        loginCtrl.SetUp(loginModal, _socketManager);
        hostRoomCtrl.SetUp(Modals.instance.GetModal<HostRoomModal>(), _socketManager);
        monitorCtrl.SetUp(_socketManager);
        userInfoCtrl.SetUp(_socketManager, Modals.instance.GetModal<UserInfoModal>());
        classScoreCtrl.SetUp(Modals.instance.GetModal<ClassInfoModal>());
        //hostRoomCtrl.Process();
        loginCtrl.ProcessLogin();
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
        _socketManager.Emit(TypeFlag.SocketEvent.Disconnect);

        string leaveAPI = StringAsset.GetFullAPIUri(string.Format(Expect.StaticAsset.StringAsset.API.ManualDisconnect, _socketManager.originalSocketID));
        Debug.Log(leaveAPI);
        //APIHttpRequest.Curl(leaveAPI, BestHTTP.HTTPMethods.Get, null, null);
        //APIHttpRequest.NativeCurl(leaveAPI, UnityWebRequest.kHttpVerbGET, null, null, null);
        StartCoroutine( APIHttpRequest.NativeCurl(leaveAPI, UnityWebRequest.kHttpVerbGET, null, null, null));
        //_socketManager.socket.Disconnect();
    }
}
