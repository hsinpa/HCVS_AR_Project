using HTC.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modals : MonoBehaviour
{
    BaseView[] modals;

    public void SetUp() {
        modals = GetComponentsInChildren<BaseView>();
    }

    public T GetModal<T>() where T : BaseView {
        return GetComponentInChildren<T>();
    }

    public T OpenModal<T>() where T : BaseView {
        if (modals == null) return null;

        BaseView targetModal = null;

        foreach (BaseView modal in modals) {

            if (typeof(T) == modal.GetType())
            {
                targetModal = modal;
                targetModal.Show(true);
            }
            else {
                modal.Show(false);
            }
        }

        return targetModal as T;
    }

    //public void Close() { 
        
    //}

    public void CloseAll()
    {
        if (modals == null) return;

        foreach (var modal in modals)
        {
            modal.Show(false);
        }
    }
}
