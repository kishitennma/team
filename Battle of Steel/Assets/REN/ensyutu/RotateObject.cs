using UnityEngine;
public class RotateObject : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 0); //����10�x����]�i�����\�j
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime); //���t���[����]
    }
}