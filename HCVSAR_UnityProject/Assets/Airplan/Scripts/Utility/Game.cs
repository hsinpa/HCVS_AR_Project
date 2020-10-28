using UnityEngine;
using System.Collections;

// 此腳本為共通資料，全域變數、全域函數請放置在這裡
public static class Game {
	public static Save sav = new Save() ;
    public static Transform moveall; //前進的物件 會將所有跟著移動的物件 變成他的子物件

    public static void ShowCursor(bool show){
		Cursor.visible = show ;
		Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked ;
	}
}

// 存檔類別，需要儲存的資料放這
public class Save{
	
}

public class DragonBody{
	public Transform body;			// 身體Transform
	public Vector3[] positions ;	// 位置陣列
	public Quaternion[] rotations ; // 旋轉陣列
	public float distance = 1.3f;	// 與"前"一節的間距
}
