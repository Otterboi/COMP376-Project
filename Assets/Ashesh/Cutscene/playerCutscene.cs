using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class playerCutscene : MonoBehaviour
{
    public Transform outsidePosition; // Outside Building
    public float delayBeforeOutside = 3f; // Delay before reappearing outside

    public float walkSpeed = 0.5f;
    public float runSpeed = 4f;

    public float walkDistance = 1.5f;
    public float runDistance = 5.5f; // Before and after jumping

    public float dashDistance = 3f; // Distance to dash
    public float jumpHeight = 1f; // Height of the jump

    public CanvasGroup blackScreenCanvas; // Reference to the Canvas for the black screen
    public float fadeDuration = 5f;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI continueText; // Press A to continue

    public CanvasGroup dialogueCanvas; // Reference to the Canvas for the dialogue box
    public TextMeshProUGUI dialogueText;
    public float dialoguePause = 12f;

    private Animator animator;
    private TrailRenderer trailRenderer;
    private CameraScript cameraScript; // Reference to the CameraScript
    public FadeInOut fade;

    void Start()
    {
        animator = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
        cameraScript = Camera.main.GetComponent<CameraScript>(); // Assumes the main camera has the CameraScript attached
        dialogueCanvas.alpha = 0; // Hide dialogue panel
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Pause the game
        Time.timeScale = 0;

        // Display black screen and log text
        blackScreenCanvas.alpha = 1;
        yield return StartCoroutine(TypeText("Log: Day 75\n" +
            "I don't know why I keep doing this.\n" +
            "What's all of this gonna be good for?\n" +
            "It's been months since this Sphere thing appeared,\n" +
            "I haven't seen another living creature in weeks,\n" +
            "not since Ash was...\n" +
            "taken... \n" +
            "Why even bother with these logs anymore,\n" +
            "if no one will be left to read them.\n" +
            "- Astrid", 0.09f, 0.5f));

        // Display continueText
        continueText.alpha = 1;
        continueText.text = "Press any key to continue";

        // Wait for A button to be pressed 
        yield return StartCoroutine(WaitForKeyPress());
        continueText.alpha = 0; // Hide continueText
        logText.alpha = 0; // Hide logText

        yield return StartCoroutine(FadeOutBlackScreen()); // Fade out black screen
        Time.timeScale = 1;

        // Start the camera script
        cameraScript.StartCameraSequence();

        // Sleep (idle) Animation time
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Trigger Player wake-up animation and wait for animation to finish
        animator.SetTrigger("WakeUp");
        StartCoroutine(DisplayDialogue("Alright...", "Let's get to work", 0.05f, 1f, 1f));  // Display dialogue without delaying sequence
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

        // Trigger walking animation and move player up 
        animator.SetTrigger("WalkUp");
        yield return StartCoroutine(MovePlayer(Vector3.up, walkSpeed, walkDistance / 2f));

        // Delay before player is outside
        yield return new WaitForSeconds(delayBeforeOutside);

        // Set player outside and trigger walk animation
        transform.position = outsidePosition.position;
        animator.SetTrigger("Outside");
        yield return StartCoroutine(MovePlayer(Vector3.right, walkSpeed, walkDistance));

        // Trigger look around animation
        animator.SetTrigger("Look");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 3.5f);

        // Trigger running animation
        animator.SetTrigger("Run");
        yield return StartCoroutine(MovePlayer(Vector3.right, runSpeed, runDistance));

        // Trigger jump animation
        animator.SetTrigger("Jump");
        yield return StartCoroutine(JumpAndDash(jumpHeight, dashDistance));

        // Trigger running animation
        animator.SetTrigger("Run");
        yield return StartCoroutine(MovePlayer(Vector3.right, runSpeed, runDistance));

        fade.FadeIn();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Level 1");

    }

    // Method to display text as if typing it
    IEnumerator TypeText(string text, float typeSpeed, float displayDuration)
    {
        logText.text = "";
        foreach (char c in text)
        {
            logText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        yield return new WaitForSecondsRealtime(displayDuration);
    }

    // Method to display text panel and dialogue
    IEnumerator DisplayDialogue(string initialText, string finalText, float typeSpeed, float initialDisplayDuration, float finalDisplayDuration)
    {
        yield return new WaitForSecondsRealtime(2f); // Pause to sync to WakeUp animation

        dialogueCanvas.alpha = 1;

        // Display initial text
        yield return StartCoroutine(TypeTextDialogue(initialText, typeSpeed));
        yield return new WaitForSecondsRealtime(initialDisplayDuration);

        // Overwrite with final text after a pause
        yield return new WaitForSecondsRealtime(dialoguePause);

        dialogueText.text = "";
        yield return StartCoroutine(TypeTextDialogue(finalText, typeSpeed));
        yield return new WaitForSecondsRealtime(finalDisplayDuration);

        dialogueCanvas.alpha = 0;
    }

    IEnumerator TypeTextDialogue(string text, float typeSpeed)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
    }

    // Method to wait for key press, key = A 
    IEnumerator WaitForKeyPress()
    {
        while (!Input.anyKey)
        {
            yield return null;
        }
    }

    // Method to fade out Black Screen
    IEnumerator FadeOutBlackScreen()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            blackScreenCanvas.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore time scale
            yield return null;
        }
        blackScreenCanvas.alpha = 0;
    }

    IEnumerator MovePlayer(Vector3 direction, float speed, float distance)
    {
        float movedDistance = 0f;

        while (movedDistance < distance)
        {
            float moveStep = speed * Time.deltaTime;
            transform.Translate(direction * moveStep);
            movedDistance += moveStep;
            yield return null;
        }
    }

    IEnumerator JumpAndDash(float height, float dashDistance)
    {
        float jumpDuration = 0.5f;
        float elapsedTime = 0f;

        Vector2 startPosition = transform.position;
        Vector2 peakPosition = startPosition + Vector2.up * height;
        Vector2 endPosition = peakPosition + Vector2.right * dashDistance;

        // Jump peak
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = Vector2.Lerp(startPosition, peakPosition, elapsedTime / (jumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Dash at peak height and enable trail renderer
        trailRenderer.emitting = true;
        elapsedTime = 0f;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = Vector2.Lerp(peakPosition, endPosition, elapsedTime / (jumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Descend
        elapsedTime = 0f;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = Vector2.Lerp(endPosition, new Vector2(endPosition.x, startPosition.y), elapsedTime / (jumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        trailRenderer.emitting = false; // Disable trail renderer after dash
    }
}