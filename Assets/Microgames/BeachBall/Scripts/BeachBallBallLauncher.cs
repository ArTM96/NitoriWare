﻿using UnityEngine;

/// <summary>
/// Observes ball params. Launches the ball backwards and triggers loss if requirements are met
/// </summary>
public class BeachBallBallLauncher : MonoBehaviour
{
    //[Header("Deprecated physics launch properties")]
    //public float ThrowForce = 500f;

    [Header("Launch equation arg (affects gravity scale and throw force)")]
    public float ThrowMultiplier = 8f;

    [Header("Launch equation arg (affects height)")]
    public float ThrowConstant = 442f;

    public AudioClip launchSound;

    public Vector2 ThrowDirection = new Vector2(0, 1);

    private Rigidbody2D physicsModel;
    private BeachBallScaler scaleMultiplier;

    private BoxCollider2D ballStandCollider;
    private Animation sealAnimation;
    private Animation ballAnimation;

    public bool Launched { get; private set; }

    void Start()
    {
        physicsModel = GetComponent<Rigidbody2D>();
        scaleMultiplier = GetComponent<BeachBallScaler>();
        ballAnimation = GetComponent<Animation>();
        //ballStandCollider = GameObject.Find("BallStand").GetComponent<BoxCollider2D>();
        sealAnimation = GameObject.Find("Seal").GetComponent<Animation>();
    }

    void Update()
    {
        if (!Launched && Input.GetKeyDown(KeyCode.Space))
        {
            Launched = true;
            //start scaling
            scaleMultiplier.Started = true;
            //animate the seal
            sealAnimation.Play();

            //throw the ball using physics
            //ThrowForce = ThrowConstant * Sqrt(ThrowMultiplier) (obtained using power curve fitting)
            physicsModel.gravityScale = ThrowMultiplier;
            physicsModel.AddForce(ThrowDirection.normalized *
                (float)System.Math.Sqrt(ThrowMultiplier) * ThrowConstant);

            //set triggerMode to prevent collisions when the ball falls
            //ballStandCollider.isTrigger = true;

            MicrogameController.instance.playSFX(launchSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(transform.position.x));
        }
    }
}
