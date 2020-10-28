using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSchedule : MonoBehaviour {
    //public ScheduleStyle SS;
    //public GameObject[] k;
    public timeboxs[] Timeboxs;
    public LockMonter LM;
    public GameObject LVUP;
    public Transform MoveAll;//前進的物件 會將所有跟著移動的物件 變成他的子物件
    float CD=10;
    Player pl;

    void Awake()
    {
        Game.moveall = MoveAll;
        pl = Object.FindObjectOfType<Player>();
    }

    void Start () {
       // SS.produce(k);
       // k = SS.obj();
        
	}
	
	
	void Update () {

        if (LM.monsters.Length <= 0 && CD <= 0)
        {
            changebox();
            CD = 10f;
        } else if (CD <= -180) 
        {
            changebox();
            CD = 10f;
        }
        CD -= 1 * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.P))
        {
            pl.SpawnBody();
        }
	}

    int boxNumber = 0;
    public void changebox()
    {
        Timeboxs[boxNumber].stop();
        for(int i =0; i< Timeboxs[boxNumber].LV; i++)
        {
            pl.SpawnBody();
        }
        boxNumber++;
        Timeboxs[boxNumber].start();
    }


}

[System.Serializable]
public class timeboxs{
    public GameObject[] timebox;
    public int LV;

    public void start()
    {
        foreach(GameObject t in timebox)
        {
            t.SetActive(true);
        }
    }

    public void stop()
    {
        foreach (GameObject t in timebox)
        {
            t.SetActive(false);
        }
    }
}
