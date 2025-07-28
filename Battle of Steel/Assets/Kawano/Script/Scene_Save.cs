using UnityEngine;

public class Scene_Save : MonoBehaviour
{
    public static int Scene_Value = -1;//リトライ時のシーン保存
    
    public void Scene_Saving_Value(int valie)
    {
        Scene_Value = valie;//値保存
    }
}