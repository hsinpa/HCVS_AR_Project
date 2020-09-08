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
        [SerializeField]
        public Transform container;
        public Transform selectAnswer;

        public delegate void OnButtonClick();
        public event OnButtonClick buttonClick;

        private int currentSelectIndex;
        private int wrongSelectIndex = -1;
        private int _correctAnswer;
        private int missionScore;
        private bool isSelectOnce;

        public void QuestionView(string question, string[] answer,int correctAnswer, TypeFlag.SocketDataType.StudentType studentScoreData)
        {
            Questions.text = question;
            AnswerText(answer, studentScoreData);
            _correctAnswer = correctAnswer;
        }

        private void AnswerText(string[] answer, TypeFlag.SocketDataType.StudentType studentScoreData)
        {
            float height = 40f;

            for (int i = 0; i < answer.Length; i++)
            {
                Transform selectTransform = Instantiate(selectAnswer, container);
                RectTransform selectRectTransform = selectTransform.GetComponent<RectTransform>();

                selectRectTransform.anchoredPosition = new Vector2(0, -height * i);
                selectTransform.Find("Answer").GetComponent<Text>().text = answer[i];

                selectTransform.gameObject.SetActive(true);
            }

            SelectButton(studentScoreData);
        }

        private void SelectButton(TypeFlag.SocketDataType.StudentType studentScoreData)
        {
            for (int i = 0; i < SelectButtons.Length; i++)
            {
                int index = i;
                SelectButtons[index].onClick.AddListener(() => AddSelectListener(index));
            }

            confirmButton.GetComponent<Button>().onClick.AddListener(() => Confirm(studentScoreData));
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

        private void Confirm(TypeFlag.SocketDataType.StudentType studentScoreData)
        {
            if (currentSelectIndex == _correctAnswer && isSelectOnce == false)
            {
                missionScore = 15;
                studentScoreData.score = missionScore;
                confirmButton.SetActive(false);
                nextButton.SetActive(true);
                PostScoreEvent.Instance.PostScore(studentScoreData, true);
                ShowCorrectOption();

                Debug.Log("Correct!!!  Get Score: " + missionScore);
            }
            else if (currentSelectIndex == _correctAnswer && isSelectOnce)
            {
                missionScore = 10;
                studentScoreData.score = missionScore;
                confirmButton.SetActive(false);
                nextButton.SetActive(true);
                PostScoreEvent.Instance.PostScore(studentScoreData, true);
                ShowCorrectOption();

                Debug.Log("Correct!!!  Get Score: " + missionScore);
            }
            else if (currentSelectIndex != _correctAnswer && isSelectOnce == false)
            {
                isSelectOnce = true;
                wrongSelectIndex = currentSelectIndex;
                SelectButtons[currentSelectIndex].image.sprite = SelectFalse;
                SelectButtons[currentSelectIndex].interactable = false;

                Debug.Log("Wrong Once!!!");
            }
            else
            {
                missionScore = 0;
                studentScoreData.score = missionScore;
                confirmButton.SetActive(false);
                nextButton.SetActive(true);
                PostScoreEvent.Instance.PostScore(studentScoreData, false);
                ShowCorrectOption();
                Debug.Log("Wrong!!!!!!!");
            }

        }

        

    }
}
