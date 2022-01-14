using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPolyManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject ambientSFX;
    public EnemyBulletSpawner[] enemy;
    public GameObject camera;

    private bool movementActive;
    private bool animActive;
    private bool sfxActive;
    private bool particleActive;
    private bool cameraActive;
    private bool lightActive;

    private void Start()
    {
        Debug.Log("Start");
        enemy = GameObject.FindObjectsOfType<EnemyBulletSpawner>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Movement");
            if (!movementActive)
            {
                player.betterMovement = true;
                movementActive = true;
            }
            else
            {
                player.betterMovement = false;
                movementActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Animation");

            if (!animActive)
            {
                player.activateAnim = true;
                foreach (EnemyBulletSpawner enmy in enemy)
                {
                    enmy.activateAnim = true;
                }
                animActive = true;
            }
            else
            {
                foreach (EnemyBulletSpawner enmy in enemy)
                {
                    enmy.activateAnim = false;
                }
                player.activateAnim = false;
                animActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("SFX");

            if (!sfxActive)
            {
                player.activateSFX = true;
                foreach(EnemyBulletSpawner enmy in enemy)
                {
                    enmy.activateSFX = true;
                }
                foreach(AudioSource source in ambientSFX.GetComponents<AudioSource>())
                {
                    source.Play();
                }
                sfxActive = true;
            }
            else
            {
                player.activateSFX = false;
                foreach (EnemyBulletSpawner enmy in enemy)
                {
                    enmy.activateSFX = false;
                }
                foreach (AudioSource source in ambientSFX.GetComponents<AudioSource>())
                {
                    source.Pause();
                }
                sfxActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Particle");

            if (!particleActive)
            {
                player.gameObject.GetComponent<ParticleSystem>().Play();
                player.gameObject.GetComponentInChildren<ParticleSystem>().Play();
                player.activeParticle = true;
                particleActive = true;
            }
            else
            {
                player.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                player.gameObject.GetComponent<ParticleSystem>().Stop();
                player.activeParticle = false;
                particleActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Camera");

            if (!cameraActive)
            {
                cameraActive = true;
                player.activeShake = true;
            }
            else
            {
                cameraActive = false;
                player.activeShake = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (!lightActive)
            {
                lightActive = true;
                camera.SetActive(true);
            }
            else
            {
                lightActive = false;
                camera.SetActive(false);
            }
        }
    }
}
