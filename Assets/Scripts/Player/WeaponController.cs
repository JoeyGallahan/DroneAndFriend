using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] Transform target;
    [SerializeField] Transform orb;
    [SerializeField] GameObject currentBulletType;
    Vector3 pos;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateReticalPosition();
        GetInput();
    }

    void UpdateReticalPosition()
    {
        pos = Input.mousePosition;
        pos.z = (target.transform.position.z - Camera.main.transform.position.z);
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos -= target.transform.position;

        angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        if (angle < 0.0f)
        {
            angle += 360.0f;
        }

        orb.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);

        float xPos = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;
        float yPos = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;

        orb.transform.localPosition = new Vector3(target.transform.position.x + xPos * 4, target.transform.position.y + yPos * 4, 0.0f);
    }

    void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet;
            bullet = GameObject.Instantiate(currentBulletType, null);
            bullet.GetComponent<BulletController>().angle = angle;
            bullet.transform.position = orb.transform.position;
        }
    }
}
