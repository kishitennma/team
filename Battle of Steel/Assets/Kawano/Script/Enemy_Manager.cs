using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_count;
    public List<GameObject> boss;//�{�X�̏o���t���O
    private bool boss_spawned = false;//�{�X�̏o���t���O
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
            //�{�X���o��
            for (int i = 0; i < boss.Count; i++)
            {
                boss[i].SetActive(true);
                remaining++;
            }
            boss_spawned = true;
            Debug.Log("�{�X�o��");
        }

        //�{�X���S�ē|���ꂽ�烊�U���g��\��
        if(boss_spawned && remaining == 0)
        {
            /*Time.timeScale = 0f; // �Q�[����~
            FindObjectOfType<ResultManager>()?.ShowResult(); // ���U���g�\���̃g���K�[
            this.enabled = false; // Enemy_Manager��Update���~�߂�i�C�Ӂj*/
           
            SceneManager.LoadScene("Result");//��������^�C�g���ɖ߂�//���U���g���
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

    //�I�v�V����:�S�Ă̓G����x�ɏ���(�f�o�b�O��A���Z�b�g�p)
    public void ClearEnemys()
    {
        enemys.Clear();
    }
}