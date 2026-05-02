using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCamera : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;

            csd.CameraPosition = currentTime / maxTime;
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
