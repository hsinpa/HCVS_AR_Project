using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Utility;
using UnityEngine.SceneManagement;

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

        public static string RandomSkinString;

        public GameObject AirPlaneGame;

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


            int hasSpecialSkin = PlayerPrefs.GetInt(GeneralFlag.PlayerPrefKey.ZeroJet_Skin, -1);
            SetSkinBtnEvent(index: 0, normalSkin);
            SetSkinBtnEvent(index: hasSpecialSkin, specialSkin);

            RandomSkinString = UtilityMethod.RollDice() == 0 ? GeneralFlag.ARZero.TrackImage_1 : GeneralFlag.ARZero.TrackImage_2;
            StartCoroutine(CheckIfNoARCore());
        }

        public void ForceUpdate() {

            Debug.Log("Force Update");


            if (OnDataUpdate != null)
                OnDataUpdate(arDataSync);
        }

        private void OnClickTakeOffBtnEvent() {
            if (OnTakeOffBtnEvent != null) OnTakeOffBtnEvent();

            arImageManager.enabled = false;

            _ = UtilityMethod.DoDelayWork(6, () =>
            {
                Debug.Log("ZeroJet : Change scene");
               // StartCoroutine(LoadYourAsyncScene());
            });
        }

        private void UpdateWebCSV() {
            StartCoroutine(arDataSync.WebSyncARData(() => {
                ForceUpdate();
            }));
        }

        private void SetSkinBtnEvent(int index, Button skinBtn) {

            if (index <= 0)
            {
                skinBtn.gameObject.SetActive(false);
                return;
            }
                

            skinBtn.onClick.AddListener(() =>
            {
                if (OnSkinChangeClick != null)
                    OnSkinChangeClick(index);
            });
        }

        private IEnumerator CheckIfNoARCore() {
            if ((ARSession.state == ARSessionState.None) ||
                (ARSession.state == ARSessionState.CheckingAvailability))
            {
                yield return ARSession.CheckAvailability();
            }

            Debug.Log(ARSession.state);

            if (ARSession.state == ARSessionState.Unsupported)
            {
                yield return LoadYourAsyncScene();
            }
        }

private IEnumerator LoadYourAsyncScene()
        {
            string TargetScene = "AirScene";
            //AirPlaneGame.SetActive(true);
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(TargetScene);

            // Wait until the asynchronous scene fully loads
            /* while (!asyncLoad.isDone)
             {
                 yield return null;
             }*/
            yield return null;
        }


    }
}