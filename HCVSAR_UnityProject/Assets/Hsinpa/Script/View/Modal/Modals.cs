using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View
{

    public class Modals : MonoBehaviour
    {
        [SerializeField]
        Image background;

        [SerializeField]
        bool hasBackground;

        BaseView[] modals;

        private static Modals _instance;

        public static Modals instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Modals>();
                    _instance.SetUp();
                }
                return _instance;
            }
        }

        private List<Modal> openModals = new List<Modal>();
        private Modal currentModals;

        public void SetUp()
        {
            modals = GetComponentsInChildren<Modal>();
        }

        public void EnableBackground(bool isEnable) {
            hasBackground = isEnable;
        }

        public T GetModal<T>() where T : Modal
        {
            return modals.First(x=> typeof(T) == x.GetType()) as T;
        }

        public T OpenModal<T>() where T : Modal
        {
            if (modals == null) return null;

            Modal targetModal = null;

            foreach (Modal modal in modals)
            {

                if (typeof(T) == modal.GetType())
                {
                    targetModal = modal;
                    targetModal.Show(true);
                }
                else
                {
                    modal.Show(false);
                }
            }

            int modalIndex = openModals.FindIndex(x => x.GetType() == typeof(T));

            if (modalIndex < 0) {
                openModals.Add(targetModal as T);
            }

            currentModals = targetModal as T;

            background.enabled = (hasBackground);

            return targetModal as T;
        }

        public void Close() {
            if (currentModals != null)
                currentModals.Show(false);

            if (openModals.Count > 0) {
                openModals.RemoveAt(openModals.Count - 1);
            }

            currentModals = (openModals.Count > 0) ? openModals[openModals.Count - 1] : null;
            background.enabled = (currentModals != null && hasBackground);
        }

        public void CloseAll()
        {
            if (modals == null) return;

            foreach (var modal in modals)
            {
                modal.Show(false);
            }

            background.enabled = false;
            openModals.Clear();
        }
    }
}