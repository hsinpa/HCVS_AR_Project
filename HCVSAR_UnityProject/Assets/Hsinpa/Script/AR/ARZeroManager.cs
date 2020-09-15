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
        public Button GoogleSheetBtn;

        // Start is called before the first frame update
        void Start()
        {
            arImageManager = GetComponent<ARTrackedImageManager>();
            arDataSync = new ARDataSync();
            UpdateWebCSV();

            if (GoogleSheetBtn != null)
                GoogleSheetBtn.onClick.AddListener(() => { UpdateWebCSV(); });
        }

        public void ForceUpdate() {

            Debug.Log("Force Update");


            if (OnDataUpdate != null)
                OnDataUpdate(arDataSync);
        }

        private void UpdateWebCSV() {
            StartCoroutine(arDataSync.WebSyncARData(() => {
                ForceUpdate();
            }));
        }
    }
}