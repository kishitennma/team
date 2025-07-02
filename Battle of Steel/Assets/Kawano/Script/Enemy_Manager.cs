using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_count;
    public List<GameObject> boss;//ボスの出現フラグリスト
    private bool boss_spawned = false;//ボスの出現フラグ
    public GameObject boss_flag_text;//ボス出現テキスト
    public static bool last_boss_spawnwd = false;//最終ボス出現フラグ
    private List<GameObject> enemys = new();//エネミーのリスト
    private int boss_set_time = 0;

    private void Awake()
    {
        //フラグ初期化
        last_boss_spawnwd = false;
        boss_spawned = false;

        boss_flag_text.SetActive(false);
        if(enemy_count == null)
        {
            enemy_count = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        int remaining = enemy_count.GetAliveEnemyCount();
        if(!boss_spawned && remaining == 0)
        {
            boss_set_time++;
            boss_flag_text.SetActive(true);
            StageChange.first_boss_spawned = true;
            if (boss_set_time > 250)
            {

                //ボスを出現
                for (int i = 0; i < boss.Count - 1; i++)
                {
                    boss[i].SetActive(true);
                    remaining++;
                }
                boss_spawned = true;
                boss_set_time = 0;
                boss_flag_text.SetActive(false);
            }
            
            Debug.Log("ボス出現");
        }
        //ボスが全て倒されたら最終ボスを出現させる
        if (!last_boss_spawnwd && boss_spawned && remaining == 0)
        {
            boss_set_time++;
            boss_flag_text.SetActive(true);
            if(boss_set_time > 300)
            {
                remaining++;
                boss[boss.Count - 1].SetActive(true);
                
                boss_set_time = 0;
                boss_flag_text.SetActive(false);
            }
        }
        //ボスも全て倒されたらリザルトを表示
        if(boss_spawned && last_boss_spawnwd && remaining == 0)
        {
            SceneManager.LoadScene("Result");//いったんタイトルに戻る//リザルト画面
            //カーソルを元に戻す
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Enemy_Controller.count_game_state+=1;//クリア回数に応じて敵ステータス強化
        }
    }
    
    public void RegisterEnemy(GameObject enemy)
    {
        if(!enemys.Contains(enemy))
        {
            enemys.Add(enemy);
        }
    }
    public void UnregisterEnemy(GameObject enemy)
    {
        enemys.Remove(enemy);
    }
    public int GetAliveEnemyCount()
    {
        return enemys.Count;
    }

    //オプション:全ての敵を一度に消す(デバッグや、リセット用)
    public void ClearEnemys()
    {
        enemys.Clear();
    }
}