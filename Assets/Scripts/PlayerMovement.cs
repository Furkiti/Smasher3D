using System;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float velocity;

    public float loadDistance;
    public float flyDistance;
    public float bumpedDistance;

    private float existPosition;
    private bool fovDecreasing;

    public float maxSpeed;
    [SerializeField] private float maxAnimationSpeed;
    [SerializeField] private float loadingSpeedMultiplier;
    [SerializeField] private float flySpeedMultiplier;
    [SerializeField] private float cameraFov;

    [SerializeField] private Animator PlayerAnimation;
    [SerializeField] private Camera PlayerCamera;

    public enum PlayerState { Running, Loading, Flying, Bumped }

    public PlayerState state;

    private bool GettingInput = true; 

    void Start()
    {
        //EventManager.Instance.NextLevelCollider += NextLevelCollider;
        EventManager.Instance.WallCollision += BumpWall;
        EventManager.Instance.PlayerSmashedWall += SlowDownPlayer;
        EventManager.Instance.CollideObstacle += CollideObstacle;

        state = PlayerState.Running;

        PlayerAnimation.SetBool("IsRunning", true);

        fovDecreasing = false;
    }
    
    void FixedUpdate()
    {
        if(GettingInput == true)
        PlayerInput();

        PlayerStates();
    }

    void PlayerInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == PlayerState.Running)
            {
                existPosition = transform.position.z;

                state = PlayerState.Loading;

                PlayerAnimation.speed = 1f;

                PlayerAnimation.SetBool("IsSwelling", true);

                PlayerAnimation.SetFloat("RunSpeed", 2f);
                PlayerAnimation.SetFloat("SwellingSpeed", 1f - Mathf.Clamp01(1f - (speed / maxSpeed)));
            }
        }

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (state == PlayerState.Running)
                {
                    existPosition = transform.position.z;

                    state = PlayerState.Loading;

                    PlayerAnimation.speed = 1f;

                    PlayerAnimation.SetBool("IsSwelling", true);

                    PlayerAnimation.SetFloat("RunSpeed", 2f);
                    PlayerAnimation.SetFloat("SwellingSpeed", 1f - Mathf.Clamp01(1f - (speed / maxSpeed)));
                }
            }

           
        }
    }

    void PlayerStates()
    {
        if (state == PlayerState.Running)
        {
            // Speeding up
            speed = Mathf.Clamp(speed + velocity, 1f, maxSpeed);

            PlayerAnimation.SetFloat("RunSpeed", Mathf.Clamp(1f + (speed * ((maxAnimationSpeed - 1f) / maxSpeed)), 1f, maxAnimationSpeed));
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (state == PlayerState.Loading)
        {
            if (transform.position.z - existPosition > loadDistance / (1f + (speed / maxSpeed)))
            {
                state = PlayerState.Flying;
                PlayerAnimation.SetBool("IsSwelling", false);
                existPosition = transform.position.z;
                fovDecreasing = true;

                return;
            }

            PlayerCamera.fieldOfView = Mathf.Clamp(PlayerCamera.fieldOfView + cameraFov * ((speed / maxSpeed) * 5), 60f, 80f);
            transform.position += transform.forward * (speed / loadingSpeedMultiplier) / (1f + (speed / maxSpeed)) * Time.deltaTime;
        }
        else if (state == PlayerState.Flying)
        {
            if (transform.position.z - existPosition > flyDistance * (1f + (speed / maxSpeed)))
            {
                state = PlayerState.Running;
                PlayerAnimation.SetBool("IsRunning", true);
                existPosition = transform.position.z;

                fovDecreasing = false;

                PlayerCamera.fieldOfView = 60;

                return;
            }

            if (fovDecreasing)
            {
                PlayerCamera.fieldOfView -= cameraFov * ((speed / maxSpeed) * 10);
            }

            transform.position += transform.forward * (speed * flySpeedMultiplier) * (1f + (speed / maxSpeed)) * Time.deltaTime;
        } else if (state == PlayerState.Bumped)
        {
            if (transform.position.z - existPosition > bumpedDistance)
            {
                state = PlayerState.Running;
                PlayerAnimation.SetBool("IsRunning", true);
                existPosition = transform.position.z;

                fovDecreasing = false;

                PlayerCamera.fieldOfView = 60;

                return;
            }

            transform.position += transform.forward * (speed * flySpeedMultiplier) * (1f + (speed / maxSpeed)) * Time.deltaTime;
        }
    }
    
    private void CollideObstacle()
    {
        GettingInput = false;
    }


    private void SlowDownPlayer()
    {
        state = PlayerState.Bumped;
        existPosition = transform.position.z;
    }

    private void BumpWall()
    {
        state = PlayerState.Bumped;
        PlayerAnimation.speed = 1f;
    }
}
