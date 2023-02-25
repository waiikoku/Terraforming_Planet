using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform camTransform;

    public float movementSpeed;
    public float normalSpeed = 1;
    public float fastSpeed = 2;
    private float movementTime = 5;

    [SerializeField]  private float rotationAmount = 10;
    [SerializeField]  private Vector3 minRot = new Vector3(35,-360,0);
    [SerializeField]  private Vector3 maxRot = new Vector3(90,360,0);

    [SerializeField]  private Vector3 zoomAmount = new Vector3(0, 1, -1);
    [SerializeField]  private Vector3 minZoom =new Vector3(0,10,-25);
    [SerializeField]  private Vector3 maxZoom = new Vector3(0,25,-10);

    [SerializeField] private Vector3 minBoundary;
    [SerializeField] private Vector3 maxBoundary;

    [Header("Tilt & Rotate")]
    public bool isTiltOrRotate = false;
    private bool[] isButtonUp = new bool[2];
    private Vector3 keyboardPos;
    private Vector3 mousePos;
    private float originCamRotX;
    private float originCamRotY;
    private float camRotX;
    private float camRotY;

    [Header("Calculation")]
    private Vector3 newPos;
    private Quaternion newRot;
    private Vector3 newZoom;
    private void Start()
    {
        instance = this;
        movementSpeed = normalSpeed;
        newPos = transform.position;
        newRot = transform.rotation;
        if (camTransform != null)
        {
            newZoom = camTransform.localPosition;
            if (camTransform.rotation != Quaternion.Euler(Vector3.zero))
            {
                originCamRotX = camTransform.rotation.eulerAngles.x;
                camRotX += originCamRotX;
            }
        }
    }

    private void Update()
    {
        isTiltOrRotate = checkorarray(isButtonUp);
        KeyboardInput();
        MouseInput();
        camRotX = Mathf.Clamp(camRotX, minRot.x, maxRot.x);
        if (isTiltOrRotate)
        {
            keyboardPos = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), 0);
            mousePos = new Vector3(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
            if (keyboardPos.y != 0)
            {
                newRot *= Quaternion.Euler((Vector3.up * keyboardPos.y) * rotationAmount);
            }
            if (keyboardPos.x != 0)
            {
                if (keyboardPos.x > 0 && camRotX < maxRot.x)
                {
                    camRotX += keyboardPos.x;
                }
                if (keyboardPos.x < 0 && camRotX > minRot.x)
                {
                    camRotX += keyboardPos.x;
                }
            }
            if (mousePos.y != 0)
            {
                newRot *= Quaternion.Euler((Vector3.up * mousePos.y) * rotationAmount);
            }
            if (mousePos.x != 0)
            {
                if (mousePos.x > 0 && camRotX < maxRot.x)
                {
                    camRotX += mousePos.x;
                }
                if (mousePos.x < 0 && camRotX > minRot.x)
                {
                    camRotX += mousePos.x;
                }

                if (Time.timeScale != 0)
                {
                    camTransform.localRotation = Quaternion.Euler(camRotX, 0, 0);
                }
                else
                {
                    camTransform.localRotation = Quaternion.Euler(camRotX, camRotY, 0);
                }
            }
            camTransform.localRotation = Quaternion.Euler(camRotX, 0, 0);
            if (Time.timeScale == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.unscaledDeltaTime * movementTime);
                if (camTransform != null)
                {
                    newZoom = new Vector3(0, Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y), Mathf.Clamp(newZoom.z, minZoom.z, maxZoom.z));
                    camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.unscaledDeltaTime * movementTime);
                }
            }
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            if (keyboardPos != Vector3.zero)
            {
                keyboardPos = Vector3.zero;
            }
            if (mousePos != Vector3.zero)
            {
                mousePos = Vector3.zero;
            }
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            if (Time.timeScale == 0)
            {
                transform.position = Vector3.Lerp(transform.position, newPos, Time.unscaledDeltaTime * movementTime);
                if (camTransform != null)
                {
                    newZoom = new Vector3(0, Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y), Mathf.Clamp(newZoom.z, minZoom.z, maxZoom.z));
                    camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.unscaledDeltaTime * movementTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        newPos = new Vector3(Mathf.Clamp(newPos.x, minBoundary.x, maxBoundary.x), newPos.y, Mathf.Clamp(newPos.z, minBoundary.z, maxBoundary.z));
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * movementTime);
        if (camTransform != null)
        {
            newZoom = new Vector3(0, Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y), Mathf.Clamp(newZoom.z, minZoom.z, maxZoom.z));
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isButtonUp[0] = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isButtonUp[0] = false;
        }

        if (!isTiltOrRotate)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPos += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPos += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPos += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPos += (transform.right * -movementSpeed);
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRot *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRot *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }
    }

    private void MouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += (-(Input.mouseScrollDelta.y) * zoomAmount);
        }

        if (Input.GetMouseButtonDown(2))
        {
            isButtonUp[1] = true;
        }

        if (Input.GetMouseButton(2))
        {

        }

        if (Input.GetMouseButtonUp(2))
        {
            isButtonUp[1] = false;
        }
    }

    private bool checkorarray(bool[] target)
    {
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i])
            {
                return true;
            }
        }
        return false;
    }
}
