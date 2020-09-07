using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Expect.StaticAsset;
using System.Text;
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

        private string[] Answers = { StringAsset.MissionsAnswer.Three.ans1, StringAsset.MissionsAnswer.Three.ans2,
                                 StringAsset.MissionsAnswer.Three.ans3, StringAsset.MissionsAnswer.Three.ans4};
        private TypeFlag.SocketDataType.LoginDatabaseType loginData;
        private int currentSelectIndex;
        private int wrongSelectIndex = -1;
        private int correctAnswer = 1;
        private int missionScore;
        private bool isSelectOnce;

        private TypeFlag.SocketDataType.StudentType studentScoreData = new TypeFlag.SocketDataType.StudentType();

        public void QuestionView(string question, string[] answer, string mission_id)
        {
            loginData = MainView.Instance.loginData;
            studentScoreData.student_id = loginData.user_id;
            studentScoreData.mission_id = mission_id;

            Questions.text = question;
            AnswerText(answer);
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

            SelectButtons[correctAnswer].interactable = true;
            SelectButtons[correctAnswer].image.sprite = SelectTrue;
        }

        private void Confirm()
        {
            if (currentSelectIndex == correctAnswer && isSelectOnce == false)
            {
                missionScore = 15;
                PostScore(missionScore);
                ShowCorrectOption();

                Debug.Log("Correct!!!  Get Score: " + missionScore);
            }
            else if (currentSelectIndex == correctAnswer && isSelectOnce)
            {
                missionScore = 10;
                PostScore(missionScore);
                ShowCorrectOption();

                Debug.Log("Correct!!!  Get Score: " + missionScore);
            }
            else if (currentSelectIndex != correctAnswer && isSelectOnce == false)
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
                PostScore(missionScore);
                ShowCorrectOption();
                Debug.Log("Wrong!!!!!!!");
            }

        }

        private void PostScore(int score)
        {
            studentScoreData.score = score;

            string jsonString = JsonUtility.ToJson(studentScoreData);

            StartCoroutine(
            APIHttpRequest.NativeCurl((StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore)), UnityWebRequest.kHttpVerbPOST, jsonString, (string success) => {
                Debug.Log("POST Success");
                MainView.Instance.PrepareScoreData(studentScoreData.student_id);
            }, () => {
                //TODO: ADD Mission ID
                Debug.Log("Error: POST Fail, Fail Mission: " + studentScoreData.mission_id);
            }));
        }

    }
}
