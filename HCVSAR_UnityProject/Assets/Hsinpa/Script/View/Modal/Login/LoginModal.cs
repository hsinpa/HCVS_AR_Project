using System.Collections;
using System.Collections.Generic;
using Expect.StaticAsset;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;
using Utility;
using System.Linq;

namespace Expect.View
{
    public class LoginModal : Modal
    {

        public enum Tab {Student, Teacher, Register}

        #region Inspector

        [Header("Tab")]

        [SerializeField]
        Transform tabs;

        LoginTabComponent[] _tabComponents;
        public List<LoginTabComponent> tabComponents => _tabComponents.ToList();

        [Header("InputField")]
        [SerializeField]
        Transform inputGroups;

        [SerializeField]
        GameObject inputComponentPrefab;

        [Header("Button")]
        [SerializeField]
        Button loginBtn;

        [SerializeField]
        Button guestBtn;

        [SerializeField]
        Text warningMsg;
        #endregion

        #region Parameter
        private Tab _currentTab;
        private System.Action<Button, Tab, List<LoginInputComponent>> _userLoginEvent;
        private System.Action _guestLoginEvent;
        private System.Action<Button, List<LoginInputComponent>> _registerEvent;

        private Dictionary<Tab, List<InputGroupStruct>> InputLookupTable;
        private List<LoginInputComponent> inputComponents = new List<LoginInputComponent>();

        #endregion

        private void Start()
        {
            _tabComponents = tabs.GetComponentsInChildren<LoginTabComponent>();
            InputLookupTable = GenerateInputTable();

            RegisterTabEvent();
            RegisterButtonEvent();

            SwitchTab(Tab.Student);
        }

        public void SetUp(System.Action<Button, Tab, List<LoginInputComponent>> UserLoginEvent, System.Action GuestLoginEvent, 
                        System.Action<Button, List<LoginInputComponent>> RegisterEvent) {
            this._userLoginEvent = UserLoginEvent;
            this._guestLoginEvent = GuestLoginEvent;
            this._registerEvent = RegisterEvent;
        }

        public void SwitchTab(Tab tab) {
            int tabIndex = tabComponents.FindIndex(x => x.tabType == tab);
            if (tabIndex < 0) return;

            SetTabEvent(_tabComponents[tabIndex].tabButton, tab, InputLookupTable[tab]);
        }

        private void RegisterTabEvent()
        {
            int tabCount = _tabComponents.Length;

            for (int i = 0; i < tabCount; i++) {
                int index = i;

                _tabComponents[i].tabButton.onClick.RemoveAllListeners();
                _tabComponents[i].tabButton.onClick.AddListener(() =>
                {
                    if (InputLookupTable.TryGetValue(_tabComponents[index].tabType, out var inputs)) {
                        SetTabEvent(_tabComponents[index].tabButton, _tabComponents[index].tabType, inputs);
                    }
                });
            }
        }

        private void RegisterButtonEvent() {
            loginBtn.onClick.AddListener(() =>
            {
                if (_currentTab == Tab.Register)
                {
                    this._registerEvent(loginBtn, inputComponents);
                }
                else {
                    this._userLoginEvent(loginBtn, _currentTab, inputComponents);
                }
            });

            guestBtn.onClick.AddListener(() =>
            {
                this._guestLoginEvent();
            });
        }

        public void SetWarningMsg(string message) {
            warningMsg.text = (string.IsNullOrEmpty(message)) ? "" : message;
            warningMsg.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }

        private void SetTabEvent(Button tab, Tab tabType, List<InputGroupStruct> inputGroupStructs)
        {
            inputComponents.Clear();
            UtilityMethod.ClearChildObject(inputGroups);

            SetWarningMsg(null);


            int structCount = inputGroupStructs.Count;

            for (int i = 0; i < structCount; i++) {
                var inputGameObject = UtilityMethod.CreateObjectToParent(inputGroups, inputComponentPrefab).GetComponent<LoginInputComponent>();
                inputGameObject.Erase();
                inputGameObject.name = inputGroupStructs[i].label;
                inputGameObject.SetTitle(inputGroupStructs[i].label, inputGroupStructs[i].isHash);

                inputComponents.Add(inputGameObject);
            }

            _currentTab = tabType;
            HighlightTab(tab);
        }

        private void HighlightTab(Button onClickTab) {

            foreach (Transform childTab in onClickTab.transform.parent.transform) {
                Button tabBtn = childTab.GetComponent<Button>();
                tabBtn.targetGraphic.color = Color.gray;
            }

            onClickTab.targetGraphic.color = Color.white;
        }

        private Dictionary<Tab, List<InputGroupStruct>> GenerateInputTable() {
            var lookuptable = new Dictionary<Tab, List<InputGroupStruct>>();

            lookuptable.Add(Tab.Student, new List<InputGroupStruct> {
                new InputGroupStruct(StringAsset.Login.AccountInputLabel)
            });

            lookuptable.Add(Tab.Teacher, new List<InputGroupStruct> {
                new InputGroupStruct(StringAsset.Login.AccountInputLabel),
                new InputGroupStruct(StringAsset.Login.PasswordInputLabel, true)
            });

            lookuptable.Add(Tab.Register, new List<InputGroupStruct> {
                new InputGroupStruct(StringAsset.Login.AccountInputLabel),
                new InputGroupStruct(StringAsset.Login.StudentNameInputLabel),
                new InputGroupStruct(StringAsset.Login.ClassIDInputLabel)
            });

            return lookuptable;
        }

        public string GetValueFromInputCompArray(string key, List<LoginInputComponent> inputArray) {
            if (string.IsNullOrEmpty(key) || inputArray == null) return "";

            int index = inputArray.FindIndex(x => x.name == key);

            if (index < 0) return "";

            return inputArray[index]._inputField.text;
        }


        private struct InputGroupStruct {
            public string label;
            public bool isHash;

            public InputGroupStruct(string label, bool isHash = false) {
                this.label = label;
                this.isHash = isHash;
            }
        }
    }
}