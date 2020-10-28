using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Hsinpa.View
{

    public class MonitorItemPrefabView : MonoBehaviour
    {
        [SerializeField]
        private Text InfoText;

        [SerializeField]
        private Image avatar;

        [SerializeField]
        private Button _button;

        public Button button => _button;

        private TypeFlag.SocketDataType.StudentDatabaseType _studentDatabaseType;
        public TypeFlag.SocketDataType.StudentDatabaseType studentDatabaseType => _studentDatabaseType;

        private bool _isOnline;
        public bool isOnline => _isOnline;

        public void SetNameAndID(TypeFlag.SocketDataType.StudentDatabaseType studentDataType)
        {
            _studentDatabaseType = studentDataType;

            InfoText.text = studentDatabaseType.student_name + "\n" + studentDatabaseType.id;

            gameObject.name = studentDatabaseType.id;

            ChangeStatus(false);
        }

        public void ChangeStatus(bool isOnline)
        {
            _isOnline = isOnline;
            avatar.color = (isOnline) ? Color.white : Color.gray;
        }

        public void SetClickEvent(System.Action<MonitorItemPrefabView> clickEvent)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() => clickEvent(this));
        }

    }
}