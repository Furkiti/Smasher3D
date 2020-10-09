using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickSmash : MonoBehaviour
{
    List<GameObject> Bricks = new List<GameObject>();

    static int brickLayer = 8;

    public float forwardPower;
    [SerializeField] private float explosionSpeedMultiplier;
    [SerializeField] private float explosionForce, explosionRadius;

    private Rigidbody rb;

    private float wallStrength;
    private float playerSpeed;
    private float maxPlayerSpeed;

    private PlayerMovement.PlayerState playerState;

    [SerializeField] private GameObject gemPrefab;
    
    int flag = 0;
    private int flag2 = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Obstacle"))
        {
           EventManager.Instance.IsCollideObstacle();
        }
        
        if (other.gameObject.layer == brickLayer)
        {
            wallStrength = other.transform.parent.gameObject.GetComponent<WallScript>().Strength;

            playerState = gameObject.GetComponent<PlayerMovement>().state;
            playerSpeed = gameObject.GetComponent<PlayerMovement>().speed;
            maxPlayerSpeed = gameObject.GetComponent<PlayerMovement>().maxSpeed;

            GameObject wall = other.transform.parent.gameObject;

            rb = other.gameObject.GetComponent<Rigidbody>();

            if (isObjectInteractedBefore(rb)) return;

            if (playerState != PlayerMovement.PlayerState.Flying && flag2 == 0)
            {
                flag2 = 1;
                EventManager.Instance.WallCollider();
                return;
            }

            ShakeCamera();

            SetChildObjectsMoveable(wall);
            ExplodeWall(rb);

            EventManager.Instance.OnPlayerSmashedWall();

            playerSpeed += 5;
        } else if (other.tag.Equals("NextLevel"))
        {
            EventManager.Instance.LevelTriggered();
            flag = 1;

        }
    }

    private void ExplodeWall(Rigidbody rigidbody)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody nearbyRigidbody = nearby.GetComponent<Rigidbody>();

            if (nearbyRigidbody != null)
            {
                if (forwardPower != 0)
                    nearbyRigidbody.AddForce(0, 0, forwardPower * ((1f + playerSpeed / maxPlayerSpeed) * explosionSpeedMultiplier), ForceMode.Impulse);

                nearbyRigidbody.AddExplosionForce(explosionForce * ((1f + playerSpeed / maxPlayerSpeed) * explosionSpeedMultiplier), transform.position, explosionRadius);
                
            }
        }
    }

    private void SetChildObjectsMoveable(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            rb = child.gameObject.GetComponent<Rigidbody>();

            rb.useGravity = true;
            rb.isKinematic = false;

            //child.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private bool isObjectInteractedBefore(Rigidbody obj)
    {
        if (obj.isKinematic == false)
            return true;

        return false;
    }

    private void ShakeCamera()
    {
        AnimationsController.Instance.playerCam();
    }
}