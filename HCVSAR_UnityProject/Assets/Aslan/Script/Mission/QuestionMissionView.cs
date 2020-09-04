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
        private Button postButton;
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
        int currentSelectIndex;
        int wrongSelectIndex = -1;
        int correctAnswer = 1;
        int missionScore;
        bool isSelectOnce;

        private TypeFlag.SocketDataType.StudentType studentScoreData = new TypeFlag.SocketDataType.StudentType();

        /*
        private void Start()
        {
            var q = StringAsset.MissionsQustion.Three.q1;
            QuestionView(q);
        }
        */
        public void QuestionView(string question, string[] answer)
        {
            Questions.text = question;
            AnswerText(answer);
        }

        private void AnswerText(string[] answer)
        {
            float height = 80f;

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
                ShowCorrectOption();
                Debug.Log("Correct!!!  Get Score: " + missionScore);
                PostScore(missionScore);
            }
            else if (currentSelectIndex == correctAnswer && isSelectOnce)
            {
                missionScore = 10;
                ShowCorrectOption();
                Debug.Log("Correct!!!  Get Score: " + missionScore);
                PostScore(missionScore);
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
                ShowCorrectOption();
                Debug.Log("Wrong!!!!!!!");
            }

        }

        //TODO: enter student_id, mission
        private void PostScore(int score)
        {
            studentScoreData.student_id = "763462";
            studentScoreData.mission_id = "D";
            studentScoreData.score = score;

            postButton.onClick.AddListener(delegate { PostScoreListener(studentScoreData); });
        }

        private void PostScoreListener(TypeFlag.SocketDataType.StudentType data)
        {
            StartCoroutine(PostScore(data));
        }

        public static IEnumerator PostScore(TypeFlag.SocketDataType.StudentType formData)
        {
            string url = StringAsset.GetFullAPIUri(StringAsset.API.PostStudentScore);
            string jsonString = JsonUtility.ToJson(formData);

            Debug.Log("url: " + url);
            UnityWebRequest webPost = UnityWebRequest.Post(url, jsonString);


            if (jsonString != null)
            {
                webPost.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
                webPost.method = UnityWebRequest.kHttpVerbPOST;
                webPost.uploadHandler.contentType = "application/json";
            }

            yield return webPost.SendWebRequest();

            if (webPost.isNetworkError || webPost.isHttpError)
            {
                Debug.Log("webPost.error " + webPost.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

    }
}
