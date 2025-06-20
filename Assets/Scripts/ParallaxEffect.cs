using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float statPos; // Vi tri dau tien cua BG
    public GameObject cam;
    public float parallaxEffect; // Toc do BG chay so voi camera

    void Start()
    {
        statPos = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float camDistance = cam.transform.position.x * parallaxEffect; // 0= di chuyen cung cam, 1 = khong di chuyen
        transform.position = new Vector3(statPos + camDistance, transform.position.y, transform.position.z);
    }
}
