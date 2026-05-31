using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCamera : MonoBehaviour
{
    InputSystem_Actions inputs;

    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject zone1;
    [SerializeField] private GameObject zone2;
    [SerializeField] private GameObject zone3;
    [SerializeField] private GameObject zone4;

    [SerializeField] private float maxTime;
    private float currentTime;
    private CinemachineSplineDolly csd;
    private void Start()
    {
        csd = this.GetComponent<CinemachineSplineDolly>();
        zone1.SetActive(false);
        zone2.SetActive(false);
        zone3.SetActive(false);
        zone4.SetActive(false);

        inputs = new InputSystem_Actions();
        inputs.Enable();
        inputs.FindAction("level_cutscene_skip").started += ctx => { Debug.Log("bweh"); currentTime = maxTime; };

        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton7))
                currentTime = maxTime;

            csd.CameraPosition = currentTime / maxTime;

            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton7))
                currentTime = maxTime;
        }
        else
        {
            mainCamera.SetActive(true);
            zone1.SetActive(true);
            zone2.SetActive(true);
            zone3.SetActive(true);
            zone4.SetActive(true);
            gameObject.SetActive(false);

        }
    }
}
