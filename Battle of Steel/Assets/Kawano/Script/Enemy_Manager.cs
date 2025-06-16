using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_count;
    public List<GameObject> boss;//ボスの出現フラグ
    private bool boss_spawned = false;//ボスの出現フラグ
    private List<GameObject> enemys = new();

    private void Awake()
    {
        if(enemy_count == null)
        {
            enemy_count = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        int remaining = enemy_count.GetAliveEnemyCount();
        if(!boss_spawned && remaining == 0)
        {
            //ボスを出現
            for (int i = 0; i < boss.Count; i++)
            {
                boss[i].SetActive(true);
                remaining++;
            }
            boss_spawned = true;
            Debug.Log("ボス出現");
        }

        //ボスも全て倒されたらリザルトを表示
        if(boss_spawned && remaining == 0)
        {
            /*Time.timeScale = 0f; // ゲーム停止
            FindObjectOfType<ResultManager>()?.ShowResult(); // リザルト表示のトリガー
            this.enabled = false; // Enemy_ManagerのUpdateを止める（任意）*/
           
            SceneManager.LoadScene("Result");//いったんタイトルに戻る//リザルト画面
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