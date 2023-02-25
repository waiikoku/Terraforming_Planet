using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    public Selection sel;
    public EletricalSystem es;
    public BuildingUI bu;

    public bool ClearValue = false;

    [Header("Fake Beam")]
    public bool FakeBeamEnable = false;
    public GameObject LaserBeam;
    [SerializeField] private SkinnedMeshRenderer smrLaserbeam;
    public float duration = 1f;
    private float timer;
    private float percentage;
    [SerializeField] private bool fully;

    [Header("Particle Effect")]
    public bool ParticleEnable = false;
    public GameObject LaserFX;
    [SerializeField] private ParticleSystem psLaser;
    public bool standby = true;
    public bool activate = false;
    [SerializeField] private float warmup = 1;
    [SerializeField] private float maximumRange = 5;

    [Header("Function")]
    public float currentProgress = 0f;
    public float maxProgress = 100f;
    public float actionSpeed = 1f;
    public float timeMultipier = 2f;
    public bool fullyCharge = false;

    private void Start()
    {
        if (LaserBeam != null)
        {
            smrLaserbeam = LaserBeam.GetComponent<SkinnedMeshRenderer>();
        }

        if(LaserFX != null)
        {
            psLaser = LaserFX.GetComponent<ParticleSystem>();
            LaserFX.SetActive(false);
        }

        timer = 0;
        percentage = 0;
    }

    private void Update()
    {
        if (es.Switch)
        {
            FakeBeamEnable = true;
            if (sel.BeingSelected)
            {
                bu.timer = (currentProgress / maxProgress);
            }
        }
        else
        {
            FakeBeamEnable = false;
        }

        if (FakeBeamEnable)
        {
            FakeBeam();
            if (fully)
            {
                if (currentProgress < maxProgress)
                {
                    currentProgress += actionSpeed * Time.deltaTime;
                }
                if (currentProgress >= maxProgress)
                {
                    fullyCharge = true;
                }
            }
        }
        else
        {
            if (timer != 0)
            {
                timer -= Time.deltaTime;
                percentage = float.Parse(((timer / duration) * 100).ToString("F2"));
                smrLaserbeam.SetBlendShapeWeight(0, percentage);
                if (timer <= 0)
                {
                    timer = 0;
                    percentage = 0;
                    smrLaserbeam.SetBlendShapeWeight(0, 0);
                }
            }
        }
        if (ClearValue)
        {
            ClearValue = false;
            smrLaserbeam.SetBlendShapeWeight(0, 0);
            timer = 0;
            percentage = 0;
        }
        if (ParticleEnable)
        {
            ParticleSystemEffect();
        }
    }

    void FakeBeam()
    {
        if (LaserBeam != null)
        {
            if (smrLaserbeam != null)
            {
                if (!fully)
                {
                    timer += Time.deltaTime;
                    percentage = float.Parse(((timer / duration) * 100).ToString("F2"));
                    smrLaserbeam.SetBlendShapeWeight(0, percentage);
                    if (timer >= duration)
                    {
                        fully = true;
                        percentage = 100;
                        smrLaserbeam.SetBlendShapeWeight(0, 100);
                    }
                }
            }
        }
    }

    void ParticleSystemEffect()
    {
        if (!LaserFX.activeInHierarchy)
        {
            LaserFX.SetActive(true);
        }
        else
        {
            if (activate)
            {
                if (psLaser.startLifetime < maximumRange)
                {
                    psLaser.startLifetime += 1 * Time.deltaTime;
                }
                return;
            }
            if (standby)
            {
                if(psLaser.startLifetime > warmup)
                {
                    psLaser.startLifetime -=  1 * Time.deltaTime;
                }
                if(psLaser.startLifetime <= 0)
                {
                    psLaser.startLifetime +=  1 * Time.deltaTime;
                }
            }
        }
    }
}
