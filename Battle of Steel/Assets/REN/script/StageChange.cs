using UnityEngine;

public class StageChange : MonoBehaviour
{
    //ボス登場フラグ
    private bool boss_spawned = false;
    public GameObject Building;

    void Update()
    {
        //ボスが登場した場合
        if(boss_spawned == false)
        {
            //GameObjct型の配列Buildingsに"Building"タグが付いたオブジェクトを格納
            GameObject[] Buildings = GameObject.FindGameObjectsWithTag("Building");

            //GameObject型の変数moveに、cubesの中身を順番に取り出す
            //foreachは配列の要素の数だけループ
            foreach (GameObject move in Buildings)
            {
                //それらを下に直線移動
                //Building.transform.positionへ毎フレーム一定の数値を足す
                Building.transform.position += new Vector3(0, -1.0f, 0);

            }
        }
    }
}
