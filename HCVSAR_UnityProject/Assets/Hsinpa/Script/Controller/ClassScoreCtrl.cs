using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;

namespace Hsinpa.Controller {
    public class ClassScoreCtrl : ObserverPattern.Observer
    {

        private TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData;
        private ClassInfoModal classInfoModal;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case GeneralFlag.ObeserverEvent.ShowMonitorUI:
                    selectedRoomData = (TypeFlag.SocketDataType.ClassroomDatabaseType)p_objects[0];
                    break;
            }
        }


        public void SetUp(ClassInfoModal classInfoModal) {
            this.classInfoModal = classInfoModal;
        }

        public void ShowClassScore(string class_id) {

        }

    }
}
