using UnityEngine;
using TrafficSimulation;

public class VehicleController : MonoBehaviour
{

    WheelDrive wheelDrive;
    public int Speed;

    void Start()
    {
        wheelDrive = this.GetComponent<WheelDrive>();

    }

    void Update()
    {
        /*float acc = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");
        float brake = Input.GetKey(KeyCode.Space) ? 1 : 0;

        wheelDrive.Move(acc, steering, brake);*/
        float xAxisValue = Input.GetAxis("Horizontal") * Speed;
        float zAxisValue = Input.GetAxis("Vertical") * Speed;

        transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y, transform.position.z + zAxisValue);
    }
}
