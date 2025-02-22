using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ���� �� ��� ���� ��� ������� ��� ��� ������ TextMeshPro
using UnityEngine.SceneManagement; // ������ ������

public class CharacterState : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f; // ���� ������ �����
    public float power = 10f; // ����� �������� �������
    private int killScore = 200; // ������ �������� ��� ��� �����
    public float currentHealth { get; private set; } // ����� ������� �������

    public GameObject gameOverPanel; // ��� Panel ���� ����� ��� ��� ������
    public TextMeshProUGUI gameOverText; // ���� "Game Over" ���� ��� Panel
    public Button restartButton; // �� ����� ������ ���� ��� Panel

    private void Start()
    {
        currentHealth = maxHealth; // ��� ������ ���� �����
        gameOverPanel.SetActive(false); // ������ �� �� ��� Panel ��� ���� �� �������
    }

    public void ChangeHealth(float value)
    {
        // ����� ����� �� ������ �� ���� ��� ������ ��� 0 � maxHealth
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        Debug.Log("Current Health: " + currentHealth + " / " + maxHealth);

        UpdateHealthUI(); // ����� ����� �������� ��� �� ����� �����

        if (currentHealth <= 0)
        {
            Die(); // ������� ���� ����� ��� ������ ����� �����
        }
    }

    private void UpdateHealthUI()
    {
        // ����� ����� �������� ����� ����� (��� �� ����)
        if (transform.CompareTag("Enemy"))
        {
            UpdateEnemyHealthBar();
        }
        else if (transform.CompareTag("Player"))
        {
            UpdatePlayerHealthBar();
        }
    }

    private void UpdateEnemyHealthBar()
    {
        // ����� �� ���� ����� �� Canvas ����� �������
        Image healthBar = transform.Find("Canvas")?.GetChild(1)?.GetComponent<Image>();
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning("Health bar not found for Enemy.");
        }
    }

    private void UpdatePlayerHealthBar()
    {
        // ����� �� ����� ����� �������� (UI) ����� ��������
        Image panelHealth = LevelManager.instance?.MainCan?.Find("PanelStats")?.Find("Panel Health")?.GetComponent<Image>();
        TextMeshProUGUI textHealth = LevelManager.instance?.MainCan?.Find("PanelStats")?.Find("TextHealth")?.GetComponent<TextMeshProUGUI>();

        if (panelHealth != null)
        {
            panelHealth.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning("Panel Health not found for Player.");
        }

        if (textHealth != null)
        {
            textHealth.text = Mathf.RoundToInt((currentHealth / maxHealth) * 100) + " %";
        }
        else
        {
            Debug.LogWarning("Text Health not found for Player.");
        }
    }

    private void Die()
    {
        if (transform.CompareTag("Player"))
        {
            // ���� ����� ������ ��� ��� ������
            Debug.Log("Game Over");
            gameOverText.text = "Game Over"; // ��� ���� "Game Over"
            gameOverPanel.SetActive(true); // ����� ��� Panel �����
            restartButton.gameObject.SetActive(true); // ����� �� ����� ������
            restartButton.onClick.AddListener(RestartGame); // ��� �� ����� ������ ������� RestartGame
        }
        else if (transform.CompareTag("Enemy"))
        {
            // ����� ������ ��� ��� ����� �������
            LevelManager.instance.score += killScore;
            Destroy(gameObject);

            // ����� ����� ���� ��� ��� �����
            if (LevelManager.instance.particals.Length > 2)
            {
                Instantiate(LevelManager.instance.particals[2], transform.position, transform.rotation);
            }
        }
    }

    public void RestartGame()
    {
        // ����� ����� ������ ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

