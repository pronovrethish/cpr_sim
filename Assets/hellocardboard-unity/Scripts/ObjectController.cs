using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    bool Identified = false;
    bool ConditionQuiz = false;
    bool CheckPulse = false;
    bool Compressions = false;

    public Material InactiveMaterial;
    public Material GazedAtMaterial;

    public Text textObject;
    public Text countdownObject;
    public Text compressionTextObject;
    public Button trueButton;
    public Button falseButton;
    public GameObject pulseUI;
    public GameObject compressionsUI;
    public GameObject thankYouSound;

    public GameObject Player;
    private UserMovement CrouchedBool;

    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    float pulseTimer = 5.0f;
    float compressionTimer = 15.0f; // changed from 20 to 15
    int compressionCount = 0;
    int cycleCount = 0;

    public void Start()
    {
        CrouchedBool = Player.GetComponent<UserMovement>();
        textObject.text = "Looks like someone needs your help! Please identify the subject who needs CPR.";
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        SetMaterial(false);
    }

    public void Update()
    {
        if (CheckPulse)
        {
            pulseUI.gameObject.SetActive(true);
            if (pulseTimer > 0)
            {
                countdownObject.text = Mathf.Round(pulseTimer).ToString();
                pulseTimer -= Time.deltaTime;
            }
            else
            {
                pulseUI.gameObject.SetActive(false);
                compressionsUI.gameObject.SetActive(true);
                textObject.text = "The patient's pulse is very faint! Start chest compressions now! Compression Rate is 100 - 120 compressions per minute!";
                CheckPulse = false;
                Compressions = true;
                compressionTimer = 15.0f; // reset timer each cycle
                compressionCount = 0;
            }
        }

        if (Compressions && compressionCount > 0)
        {
            if (compressionTimer > 0)
            {
                compressionTextObject.text = "Compressions: " + compressionCount.ToString() + "\nTimer: 00:" + Mathf.Round(compressionTimer).ToString();
                compressionTextObject.fontSize = 36; // smaller font
                compressionTimer -= Time.deltaTime;
            }
            else
            {
                // cycle finished
                cycleCount++;
                if (compressionCount < 30)
                {
                    compressionsUI.gameObject.SetActive(false);
                    Compressions = false;
                    textObject.text = "❌ You killed the person!";
                    thankYouSound.gameObject.SetActive(false);
                }
                else if (cycleCount >= 4)
                {
                    compressionsUI.gameObject.SetActive(false);
                    Compressions = false;
                    thankYouSound.gameObject.SetActive(true);
                    textObject.text = "\"Thank you! You saved my life!\"";
                }
                else
                {
                    // start next cycle
                    compressionCount = 0;
                    compressionTimer = 15.0f;
                    textObject.text = "Start next compression cycle!";
                }
            }
        }
    }

    public void OnPointerEnter()
    {
        SetMaterial(true);
    }

    public void OnPointerExit()
    {
        SetMaterial(false);
    }

    public void OnPointerClick()
    {
        Debug.Log(gameObject.name);

        if (!Identified)
        {
            Identified = true;
            textObject.text = "Assess the situation. What has happened to the patient?";
            trueButton.gameObject.SetActive(true);
            falseButton.gameObject.SetActive(true);
        }
        else if (ConditionQuiz && CrouchedBool.crouched)
        {
            textObject.text = "Check the pulse and breathing of the patient for 10 seconds...";
            CheckPulse = true;
            ConditionQuiz = false;
        }
        else if (Compressions)
        {
            compressionCount++;
            // 🔥 Fix: color feedback on interaction
            _myRenderer.material.color = Color.Lerp(Color.red, Color.green, compressionCount / 30f);
        }
    }

    public void FalseQuiz()
    {
        textObject.text = "Assess properly and try again.";
    }

    public void CorrectQuiz()
    {
        ConditionQuiz = true;
        trueButton.gameObject.SetActive(false);
        falseButton.gameObject.SetActive(false);
        textObject.text = "You assesed properly. Now Crouch down to the patient's level to check their pulse.";
    }

    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}
