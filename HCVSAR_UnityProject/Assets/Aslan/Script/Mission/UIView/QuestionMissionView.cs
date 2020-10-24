using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

namespace Expect.View
{
    public class QuestionMissionView : BaseView
    {
        [Header("Text")]
        [SerializeField]
        private Text Questions;

        [Header("Buttons")]
        [SerializeField]
        private Button[] SelectButtons;
        [SerializeField]
        private GameObject confirmButton;
        [SerializeField]
        private GameObject nextButton;

        [Header("Select Sprite")]
        [SerializeField]
        private Sprite SelectDefault;
        [SerializeField]
        private Sprite SelectTrue;
        [SerializeField]
        private Sprite SelectFalse;

        [Header("Select Answer")]
        public Transform container;
        public Transform selectAnswer;

        [Header("Audio Play")]
        [SerializeField]
        private AudioSource correctSound;
        [SerializeField]
        private AudioSource errorSound;

        public delegate void OnButtonClick();
        public event OnButtonClick buttonClick;
        private bool isSelectOnce;
        private int currentSelectIndex = -1;
        private int wrongSelectIndex = -1;
        private int _correctAnswer;

        private List<Transform> selectTransformList = new List<Transform>();

        private void Init()
        {
            currentSelectIndex = -1;
            wrongSelectIndex = -1;
            isSelectOnce = false;
            Debug.Log("Init " + isSelectOnce);
            ResetButtonAndSelect();
        }

        public void QuestionView(string question, string[] answer, int correctAnswer)
        {
            Init();
            RemoveAllListeners();

            Questions.text = question;
            AnswerText(answer);
            _correctAnswer = correctAnswer;
        }

        public void RemoveListeners()
        {
            nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        private void AnswerText(string[] answer)
        {
            float height = 37f;

            for (int i = 0; i < answer.Length; i++)
            {
                Transform selectTransform = Instantiate(selectAnswer, container);
                RectTransform selectRectTransform = selectTransform.GetComponent<RectTransform>();

                selectRectTransform.anchoredPosition = new Vector2(0, -height * i);
                selectTransform.Find("Answer").GetComponent<Text>().text = answer[i];

                selectTransformList.Add(selectTransform);
                selectTransform.gameObject.SetActive(true);
            }

            SelectButton();
        }

        private void SelectButton()
        {
            for (int i = 0; i < SelectButtons.Length; i++)
            {
                int index = i;
                SelectButtons[index].onClick.AddListener(() => AddSelectListener(index));
            }

            confirmButton.GetComponent<Button>().onClick.AddListener(() => Confirm());
            nextButton.GetComponent<Button>().onClick.AddListener(() => buttonClick());
        }

        private void AddSelectListener(int index)
        {
            ClearOption();
            currentSelectIndex = index;
            confirmButton.GetComponent<Button>().interactable = true;
            SelectButtons[index].image.sprite = SelectTrue;
            if (wrongSelectIndex >= 0) { SelectButtons[wrongSelectIndex].image.sprite = SelectFalse; }
        }

        private void ClearOption()
        {
            foreach (Button b in SelectButtons) { b.image.sprite = SelectDefault; }
        }

        private void ResetButtonAndSelect()
        {
            foreach (var t in selectTransformList) { Destroy(t.gameObject); }
            foreach (var t in selectTransformList) { Destroy(t.GetChild(0).gameObject); }

            foreach (Button b in SelectButtons) { b.image.sprite = SelectDefault; }
            foreach (Button b in SelectButtons) { b.interactable = true; }

            confirmButton.SetActive(true);
            nextButton.SetActive(false);
        }

        private void ShowCorrectOption()
        {
            foreach (Button b in SelectButtons)
            {
                b.image.sprite = SelectFalse;
                b.interactable = false;
            }

            SelectButtons[_correctAnswer].interactable = true;
            SelectButtons[_correctAnswer].image.sprite = SelectTrue;
        }

        private void Confirm()
        {
            int missionScore;
            confirmButton.GetComponent<Button>().interactable = false;

            if (currentSelectIndex == _correctAnswer)
            {
                missionScore = isSelectOnce ? 10 : 15;

                MainView.Instance.studentScoreData.score = missionScore;
                PostScoreEvent.Instance.PostScore(missionScore, MainView.Instance.loginData.userType);

                confirmButton.SetActive(false);
                nextButton.SetActive(true);

                ShowCorrectOption();
                correctSound.Play();

                Debug.Log("Correct!!!  Get Score: " + missionScore);
            }

            if (currentSelectIndex != _correctAnswer)
            {
                if (isSelectOnce)
                {
                    missionScore = 0;
                    MainView.Instance.studentScoreData.score = missionScore;
                    PostScoreEvent.Instance.PostScore(missionScore, MainView.Instance.loginData.userType);

                    confirmButton.SetActive(false);
                    nextButton.SetActive(true);

                    ShowCorrectOption();
                    errorSound.Play();
                    Debug.Log("Wrong!!!!!!!" + "isSelectOnce " + isSelectOnce);
                }
                else
                {
                    isSelectOnce = true;
                    wrongSelectIndex = currentSelectIndex;
                    SelectButtons[currentSelectIndex].image.sprite = SelectFalse;
                    SelectButtons[currentSelectIndex].interactable = false;
                    errorSound.Play();
                    Debug.Log("Wrong Once!!!" + "isSelectOnce " + isSelectOnce);
                }
            }
        }

        private void RemoveAllListeners()
        {
            foreach (var b in SelectButtons) { b.onClick.RemoveAllListeners(); }
            confirmButton.GetComponent<Button>().onClick.RemoveAllListeners();
            nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }

    }
}
