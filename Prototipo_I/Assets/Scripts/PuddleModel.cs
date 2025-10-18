using UnityEngine;

public class PuddlePartycle : MonoBehaviour
{
    public GameObject modelToShow;
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        if (modelToShow != null)
            modelToShow.SetActive(false);
    }

    private void Update()
    {
        if (ps.isPlaying)
            modelToShow.SetActive(true);
        else
            modelToShow.SetActive(false);
    }
}
