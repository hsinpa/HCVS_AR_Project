using Expect.View;
using Hsinpa.Controller;
using Hsinpa.Socket;
using Hsinpa.View;
using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainApp : Singleton<MainApp>
{
    protected MainApp() { } // guarantee this will be always a singleton only - can't use the constructor!

    private Subject subject;

    SocketIOManager _socketManager;

    private Observer[] observers = new Observer[0];

    private void Start()
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

        LoginModal loginModal = Modals.instance.GetModal<LoginModal>();
        loginCtrl.SetUp(loginModal, _socketManager);
        hostRoomCtrl.SetUp(Modals.instance.GetModal<HostRoomModal>(), _socketManager);
        monitorCtrl.SetUp(_socketManager);
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
}
