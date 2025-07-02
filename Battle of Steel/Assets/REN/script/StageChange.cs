using UnityEngine;

public class StageChange : MonoBehaviour
{
    //ボス登場フラグ
    public static bool first_boss_spawned = false;//他のスクリプトからtrueにされる
    public GameObject first_building;//下に行くオブジェクト群
    public GameObject destroy_object;//削除するオブジェクト

    private void Start()
    {
        first_boss_spawned = false;
    }

    private int counts = 0;
    void FixedUpdate()
    {
        //ボスが登場した場合
        if(first_boss_spawned == true)
        {
            Vector3 fool = new(0, 0.5f, 0);

            if(first_building != null)
            {
                if (counts < 200)
                {
                    first_building.transform.position -= fool;//下へ移動
                    destroy_object.SetActive(false);
                    counts++;
                }
                if (counts >= 200)
                {
                    first_building.SetActive(false);
                    counts = 0;
                }
            }
        }
    }
}