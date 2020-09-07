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
        private Button confirmButton;

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
        
        private int currentSelectIndex;
        private int wrongSelectIndex = -1;
        private int _correctAnswer;
        private int missionScore;
        private bool isSelectOnce;

        public void QuestionView(string question, string[] answer,int correctAnswer)
        {
            Questions.text = question;
            AnswerText(answer);
            _correctAnswer = correctAnswer;
        }

        private void AnswerText(string[] answer)
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

            SelectButton();
        }

        private void SelectButton()
        {
            for (int i = 0; i < SelectButtons.Length; i++)
            {
                int index = i;
                SelectButtons[index].onClick.AddListener(() => AddSelectListener(index));
            }

            confirmButton.onClick.AddListener(Confirm);
        }

        private void AddSelectListener(int index)
        {
            ClearOption();
            currentSelectIndex = index;
            confirmButton.interactable = true;
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

        private void Confirm()
        {
            if (currentSelectIndex == _correctAnswer && isSelectOnce == false)
            {
                missionScore = 15;
                MissionView_1.Instance.PostScore(missionScore, true);
                ShowCorrectOption();

                Debug.Log("Correct!!!  Get Score: " + missionScore);
            }
            else if (currentSelectIndex == _correctAnswer && isSelectOnce)
            {
                missionScore = 10;
                MissionView_1.Instance.PostScore(missionScore, true);
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
                MissionView_1.Instance.PostScore(missionScore, false);
                ShowCorrectOption();
                Debug.Log("Wrong!!!!!!!");
            }

        }

        

    }
}
