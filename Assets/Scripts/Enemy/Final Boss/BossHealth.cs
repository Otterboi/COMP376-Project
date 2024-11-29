using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    // Sets the health of the boss
    [SerializeField] private int hp = 200;
    [SerializeField] private Slider healthBar;
    [SerializeField] private FadeInOut fadeInOut;
	AudioSource explosion;
	[SerializeField] AudioSource hit_sound;
    public GameObject VictoryScreenUI;
    private void Start()
    {
        healthBar.maxValue = hp;
        healthBar.value = hp;
		explosion = GetComponent<AudioSource>();
    }

    // Decreases the boss's health
    public void DecreaseHP(int damage)
    {
		hit_sound.Play();
        hp -= damage;
        healthBar.value = hp;

        if(hp <= 0)
        {
            StartCoroutine(BossDefeat());
        }
    }

    private IEnumerator BossDefeat()
    {
        Cursor.visible = true;
        explosion.Play();
        yield return new WaitForSeconds(1);
        fadeInOut.FadeIn();
        yield return new WaitForSeconds(1.5f);
        VictoryScreenUI.SetActive(true);
        Cursor.visible = true;
    }
    // Setter and getter for HP
    public int Health { get { return hp; } set { hp = value; } }
}
