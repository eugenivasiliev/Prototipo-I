using System.Collections.Generic;
using UnityEngine;

public class GasPlant : PlantWeapon
{
    [SerializeField] private int damageDealt;
    [SerializeField] private float damageTime;
    private List<(IDamageable, float)> damageables = new List<(IDamageable, float)>();

    [SerializeField] private AnimationCurve throwTrajectory;
    [SerializeField] private float animationTime = 0f;
    [SerializeField] private float animationFinishTime = 0f;
    [SerializeField] private float animationSpeedMult = 1.5f;
    private bool animationFinished = false;

    public Vector3 animationStartPosition;
    public Vector3 animationDirection;

    [SerializeField] private GameObject gasParticles;
    private GameObject gasParticlesInstance;

    [SerializeField] private float lifeTime = 0f;
    [SerializeField] private float fullLifeTime = 5f;

    public override string Name => nameof(GasPlant);

    protected override void Start()
    {
        gasParticlesInstance = Instantiate(gasParticles, this.transform.position, Quaternion.identity, this.transform);
        gasParticlesInstance.transform.localScale = 0.2f * Vector3.one;
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
                gasParticlesInstance.transform.localScale = Vector3.one;
            }
        }

        lifeTime += Time.deltaTime;
        if (lifeTime > fullLifeTime)
        {
            Destroy(gasParticlesInstance);
            Destroy(this.gameObject);
            return;
        }

        for (int i = 0; i < damageables.Count; ++i)
        {
            damageables[i] = (damageables[i].Item1, damageables[i].Item2 - Time.deltaTime);
            if (damageables[i].Item2 < 0)
            {
                damageables[i].Item1.Damage(damageDealt);
                damageables[i] = (damageables[i].Item1, damageTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!animationFinished) return;
        if (collider.TryGetComponent(out IDamageable damageable)) damageables.Add((damageable, damageTime));
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!animationFinished || !collider.TryGetComponent(out IDamageable damageable)) return;
        for (int i = 0; i < damageables.Count; ++i)
        {
            if (damageables[i].Item1 == damageable)
            {
                damageables.RemoveAt(i);
                return;
            }
        }
    }
}