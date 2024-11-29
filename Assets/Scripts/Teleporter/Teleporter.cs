using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public Animator teleporterAnim;
    private FadeInOut fade;

    private void Start()
    {
        teleporterAnim = GetComponent<Animator>();
        fade = GetComponent<FadeInOut>();
    }

    public IEnumerator changeScene()
    {
        fade.FadeIn();
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Teleport()
    {
        StartCoroutine(changeScene());
    }
}
