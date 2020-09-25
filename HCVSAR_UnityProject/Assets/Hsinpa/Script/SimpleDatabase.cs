using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsinpa.Model
{
    public class SimpleDatabase : MonoBehaviour
    {
        [SerializeField]
        private MissionItemSObj _MissionItemSObj;
        public MissionItemSObj MissionItemSObj => _MissionItemSObj;

        [SerializeField]
        private MissionItemSObj _MissionShortNameObj;
        public MissionItemSObj MissionShortNameObj => _MissionShortNameObj;
    }
}