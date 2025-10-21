using System.Collections.Generic;
using UnityEngine;

public class PuddlePlant : PlantWeapon
{
    [SerializeField] private float maxSpeed;

    [SerializeField] private AnimationCurve throwTrajectory;
    [SerializeField] private float animationTime = 0f;
    [SerializeField] private float animationFinishTime = 0f;
    [SerializeField] private float animationSpeedMult = 1.5f;
    private bool animationFinished = false;

    public Vector3 animationStartPosition;
    public Vector3 animationDirection;

    [SerializeField] private GameObject puddleParticles;
    private GameObject puddleParticlesInstance;

    [SerializeField] private float lifeTime = 0f;
    [SerializeField] private float fullLifeTime = 5f;

    public override string Name => nameof(PuddlePlant);

    protected override void Start()
    {
        puddleParticlesInstance = Instantiate(puddleParticles, this.transform.position, Quaternion.identity, this.transform);
        puddleParticlesInstance.transform.localScale = 0.2f * Vector3.one;
    }

    protected override void Update()
    {
        if (!animationFinished)
        {
            animationTime += Time.deltaTime * animationSpeedMult;
            this.transform.position =
                animationStartPosition
                + animationDirection * animationTime
                + Vector3.up * throwTrajectory.Evaluate(animationTime);
            if (animationTime > animationFinishTime)
            {
                animationFinished = true;
                puddleParticlesInstance.transform.localScale = Vector3.one;
            }
        }

        lifeTime += Time.deltaTime;
        if (lifeTime > fullLifeTime)
        {
            Destroy(puddleParticlesInstance);
            Destroy(this.gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = maxSpeed;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = float.MaxValue;
    }
}