using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Remoting.Messaging;

[CreateAssetMenu(fileName = "MissionSObject", menuName = "ScriptableObjects/SpawnMissionSObj", order = 1)]
public class MissionItemSObj : ScriptableObject
{
    [SerializeField]
    public TypeFlag.InGameType.MissionType[] missionArray;

    public TypeFlag.InGameType.MissionType defaultMission {
        get {
            if (missionArray.Length <= 0)
                return default(TypeFlag.InGameType.MissionType);

            return missionArray[missionArray.Length - 1];
        }
    }

    private Dictionary<string, TypeFlag.InGameType.MissionType> _MissionTable = new Dictionary<string, TypeFlag.InGameType.MissionType>();

    public Dictionary<string, TypeFlag.InGameType.MissionType> MissionTable {
        get {

            if (missionArray == null) return _MissionTable;

            if (_MissionTable.Count <= 0) {

                int missionNum = missionArray.Length;

                for (int i = 0; i < missionNum; i++) {
                    _MissionTable.Add(missionArray[i].mission_id, missionArray[i]);
                }
            }

            return _MissionTable;
        }
    }



}