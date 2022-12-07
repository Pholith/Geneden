using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float cameraSpeed = 20f;
    public float zoomSpeed = 20f;
    public float zoomMin = 5.0f;
    public float zoomMax = 20.0f;

    [SerializeField] private Camera shaker;
    void Start()
    {
        shaker = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraInput();
    }

    private void CameraInput()
    {
        Vector3 _cameraPos = transform.position;
        float _cameraSize = shaker.orthographicSize;


        if(Input.GetKey("z"))
        {
            _cameraPos.y += cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("q"))
        {
            _cameraPos.x -= cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            _cameraPos.y -= cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            _cameraPos.x += cameraSpeed * Time.deltaTime;
        }

        float _scroll = Input.GetAxis("Mouse ScrollWheel");
        _cameraSize -= _scroll * zoomSpeed * 100f * Time.deltaTime;

        shaker.orthographicSize = _cameraSize;
        shaker.orthographicSize = Mathf.Clamp(shaker.orthographicSize, zoomMin, zoomMax);
        transform.position = _cameraPos;
    }
}
