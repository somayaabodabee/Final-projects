using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class OpenDoor : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private TextMeshProUGUI codeText;
    private string codeTextValue = "";

    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private TextMeshProUGUI instructionsText;
    public string safeCode = "";
    public GameObject codePanel;
    public GameObject collectedNumbersPanel;

    [SerializeField] private List<GameObject> allNumbers = new List<GameObject>();
    private List<GameObject> activeNumbers = new List<GameObject>();
    private List<int> collectedNumbers = new List<int>();

    void Start()
    {
        HideAllNumbers();
        SelectRandomNumbers();
        collectedNumbersPanel.SetActive(false);
        codePanel.SetActive(false);
        ShowInstructions();
    }

    void Update()
    {
        codeText.text = codeTextValue;

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha0 + i)) || Input.GetKeyDown((KeyCode)(KeyCode.Keypad0 + i)))
            {
                AddDigit(i.ToString());
            }
        }

        if (codeTextValue.Length == 4)
        {
            if (codeTextValue == safeCode)
            {
                anim.SetTrigger("OpenDoor");
                codePanel.SetActive(false);
                codeTextValue = "";
            }
            else
            {
                StartCoroutine(ResetCodeAfterDelay());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activeNumbers.Contains(other.gameObject))
        {
            CollectNumber(other.gameObject);
        }
    }

    public void AddDigit(string digit)
    {
        if (codeTextValue.Length < 4)
        {
            codeTextValue += digit;
        }
    }

    private void HideAllNumbers()
    {
        foreach (GameObject number in allNumbers)
        {
            number.SetActive(false);
        }
    }

    private void SelectRandomNumbers()
    {
        List<GameObject> shuffledNumbers = new List<GameObject>(allNumbers);
        shuffledNumbers = shuffledNumbers.OrderBy(n => Random.value).ToList();

        activeNumbers = shuffledNumbers.Take(3).ToList();

        foreach (GameObject number in activeNumbers)
        {
            number.SetActive(true);
        }
    }

    public void CollectNumber(GameObject numberObject)
    {
        TextMeshPro textComponent = numberObject.GetComponent<TextMeshPro>();

        if (textComponent != null && int.TryParse(textComponent.text, out int number))
        {
            if (!collectedNumbers.Contains(number))
            {
                collectedNumbers.Add(number);
                activeNumbers.Remove(numberObject);
                numberObject.SetActive(false);
            }

            if (collectedNumbers.Count >= 3)
            {
                SetSafeCode();
                collectedNumbersPanel.SetActive(true);
                OpenKeypad();
            }
        }
    }

    private void SetSafeCode()
    {
        collectedNumbers.Sort((a, b) => b.CompareTo(a));
        safeCode = string.Join("", collectedNumbers);
    }

    private void OpenKeypad()
    {
        Debug.Log("Opening Keypad...");
        codePanel.SetActive(true);
    }

    private void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        instructionsText.text = "Collect the numbers from largest to smallest.";
        StartCoroutine(HideInstructionsAfterDelay());
    }

    private IEnumerator HideInstructionsAfterDelay()
    {
        yield return new WaitForSeconds(3);
        instructionsPanel.SetActive(false);
    }

    private IEnumerator ResetCodeAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        codeTextValue = "";
    }
}
