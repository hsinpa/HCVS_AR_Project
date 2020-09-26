using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Hsinpa.AR
{
    public class ARZeroManager : MonoBehaviour
    {
        ARDataSync arDataSync;
        ARTrackedImageManager arImageManager;

        public System.Action<ARDataSync> OnDataUpdate;
        public System.Action<int> OnSkinChangeClick;
        public System.Action OnTakeOffBtnEvent;

        public Button GoogleSheetBtn;
        public Button TakeOffBtn;

        [Header("Skin Buttons")]
        [SerializeField]
        private Button normalSkin;

        [SerializeField]
        private Button specialSkin;

        // Start is called before the first frame update
        void Start()
        {
            arImageManager = GetComponent<ARTrackedImageManager>();
            arDataSync = new ARDataSync();
            UpdateWebCSV();
            
            if (GoogleSheetBtn != null)
                GoogleSheetBtn.onClick.AddListener(() => { UpdateWebCSV(); });

            if (TakeOffBtn != null)
                TakeOffBtn.onClick.AddListener(() => { OnClickTakeOffBtnEvent(); });

            SetSkinBtnEvent(index: 0, normalSkin);
            SetSkinBtnEvent(index: 1, normalSkin);
        }

        public void ForceUpdate() {

            Debug.Log("Force Update");


            if (OnDataUpdate != null)
                OnDataUpdate(arDataSync);
        }

        private void OnClickTakeOffBtnEvent() {
            if (OnTakeOffBtnEvent != null) OnTakeOffBtnEvent();
        }

        private void UpdateWebCSV() {
            StartCoroutine(arDataSync.WebSyncARData(() => {
                ForceUpdate();
            }));
        }

        private void SetSkinBtnEvent(int index, Button skinBtn) {
            skinBtn.onClick.AddListener(() =>
            {
                if (OnSkinChangeClick != null)
                    OnSkinChangeClick(index);
            });
        }

    }
}