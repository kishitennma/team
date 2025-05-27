using UnityEngine;
//ロックオンシステム

public class RockOn : MonoBehaviour
{
    [Header("ロックオンコンポーネント")]
    [Header("オブジェクト")]
    [SerializeField] GameObject player;//プレイヤーオブジェクト
    [Header("ロックオン設定")]
    [SerializeField] float lockon_range = 100.0f;//ロックオンの距離

    private Transform lockon_target;
    private bool is_lock_on;
    private float rotate_speed = 90;
    private bool lockon_flag = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!lockon_flag)
                lockon_flag = true;
            else
                lockon_flag = false;
        }

        if(lockon_flag == true)
        {
            if (!is_lock_on )
            {
                Set_LockOn();
            }
            else
            {
                RotateTarget();
                if (lockon_target == null)
                {
                    Stop_LockOn();
                    Debug.Log("ロックオン対象を見失いました");
                    is_lock_on = false;
                }
            }
        }
    }

    //ロックオン設定
    private void Set_LockOn()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,lockon_range);
        Transform clossest = null;
        float minDistance = Mathf.Infinity;

        foreach(Collider hit in hits)
        {
            //エネミータグなら
            if(hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    clossest = hit.transform;
                }
            }
        }
        if(clossest != null)
        {
            lockon_target = clossest;
            is_lock_on = true;
        }
    }
    //ロックオン中止
    private void Stop_LockOn()
    {
        lockon_target = null;
        is_lock_on = false;
        lockon_flag = false;
    }

    void RotateTarget()
    {
        if (lockon_target != null)
        {
            Vector3 direction = (lockon_target.position - transform.position);
            direction.y = 0;//水平方向のみ
            if (direction != Vector3.zero)
            {
                Quaternion lockrotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lockrotation, Time.deltaTime * rotate_speed);
                transform.rotation = lockrotation;
            }
        }

    }
}