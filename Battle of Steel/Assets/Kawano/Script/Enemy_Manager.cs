using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_count;
    public bool tutorial_flag;//チュートリアルフラグ
    public bool last_stage;//最終ステージフラグ
    public List<GameObject> boss;//ボスの出現フラグリスト
    private bool boss_spawned = false;//ボスの出現フラグ
    public GameObject boss_flag_text;//ボス出現テキスト
    public GameObject clear_text;//クリアテキスト
    public static bool last_boss_spawnwd = false;//最終ボス出現フラグ
    private List<GameObject> enemys = new();//エネミーのリスト
    private int boss_set_time = 0;
    private int invoke_time = 0;//遅延時間

    //定数
    const int MAX_INVOKE = 50;


    private void Awake()
    {
        //フラグ初期化
        last_boss_spawnwd = false;
        boss_spawned = false;
        clear_text.SetActive(false);

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
        int remaining = enemy_count.GetAliveEnemyCount();//ステージないにいる敵の数を参照

        //通常のワールド
        if (!tutorial_flag)
        {
            //ボスが全て倒されたら最終ボスを出現させる
            if (!last_boss_spawnwd && boss_spawned && remaining == 0)
            {
                //ラストステージの場合中ボスが３体倒されたら最終ボスを出現
                if (last_stage)
                {
                    boss_set_time++;
                    boss_flag_text.SetActive(true);
                    if (boss_set_time > 300)
                    {
                        remaining++;
                        boss[boss.Count - 1].SetActive(true);

                        boss_set_time = 0;
                        boss_flag_text.SetActive(false);
                    }

                }
                else
                {
                    clear_text.SetActive(true);
                    //クリアフラグの保持したステージ番目をクリア扱いにする
                    StageSelecting.clear_flags[Scene_Save.Scene_Value] = true;
                    //経過時間をカウント
                    invoke_time++;

                    //3秒後に画面遷移（リザルトへ移動）
                    if (invoke_time >= MAX_INVOKE)
                    {
                        //カーソルを元に戻す
                        SceneManager.LoadScene("Result");//いったんタイトルに戻る//リザルト画面

                    }
                }
            }
            //雑魚的をすべて倒したら中ボスを出現
            if (!boss_spawned && remaining == 0)
            {
                //ボスを設定していない場合はそのままリザルトへ
                if (boss.Count == 0)
                {
                    invoke_time++;
                    clear_text.SetActive(true);//クリアのテキスト表示
                   　//クリアフラグの保持したステージ番目をクリア扱いにする
                    StageSelecting.clear_flags[Scene_Save.Scene_Value] = true;

                    if (invoke_time > MAX_INVOKE)
                    {
                        SceneManager.LoadScene("Result");
                    }
                }
                else
                {
                    //ボスを設定している場合は出現させる
                    boss_set_time++;
                    boss_flag_text.SetActive(true);
                    StageChange.first_boss_spawned = true;
                    if (boss_set_time > 250)
                    {
                        //ラストステージの場合はラスボス以外を出現させる
                        if(last_stage)
                        {
                            for (int i = 0; i <= boss.Count - 2; i++)
                            {
                                boss[i].SetActive(true);
                                remaining++;
                            }
                        }
                        else
                        {
                            //ボスを出現
                            for (int i = 0; i <= boss.Count - 1; i++)
                            {
                                boss[i].SetActive(true);
                                remaining++;
                            }
                        }

                        boss_spawned = true;
                        boss_set_time = 0;
                        boss_flag_text.SetActive(false);
                    }
                    Debug.Log("ボス出現");
                }
            }
            //ボスも全て倒されたらリザルトを表示
            if (boss_spawned && last_boss_spawnwd && remaining == 0 && last_stage)
            {
                invoke_time++;
                clear_text.SetActive(true);
                //クリアフラグの保持したステージ番目をクリア扱いにする
                StageSelecting.clear_flags[Scene_Save.Scene_Value] = true;

                if (invoke_time > MAX_INVOKE)
                {
                    SceneManager.LoadScene("Result");//いったんタイトルに戻る//リザルト画面

                }
            }
        }
        else
        {
            //チュートリアル専用
            if (!boss_spawned && remaining == 0)
            {
                invoke_time++;
                clear_text.SetActive(true);
                if (invoke_time > MAX_INVOKE)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    //クリアフラグの保持したステージ番目をクリア扱いにする
                    StageSelecting.clear_flags[Scene_Save.Scene_Value] = true;

                    SceneManager.LoadScene("Result");//いったんタイトルに戻る//リザルト画面
                    invoke_time = 0;
                }
            }
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