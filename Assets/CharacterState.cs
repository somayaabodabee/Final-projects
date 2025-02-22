using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ÊÃßÏ ãä Ãäß ÊÖíİ åĞå ÇáãßÊÈÉ ÅĞÇ ßäÊ ÊÓÊÎÏã TextMeshPro
using UnityEngine.SceneManagement; // áÅÚÇÏÉ ÇáãÔåÏ

public class CharacterState : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f; // ÇáÍÏ ÇáÃŞÕì ááÕÍÉ
    public float power = 10f; // ÇáŞæÉ ÇáåÌæãíÉ ááÔÎÕíÉ
    private int killScore = 200; // ÇáäŞÇØ ÇáãßÊÓÈÉ ÚäÏ ŞÊá ÇáÚÏæ
    public float currentHealth { get; private set; } // ÇáÕÍÉ ÇáÍÇáíÉ ááÔÎÕíÉ

    public GameObject gameOverPanel; // ÇáÜ Panel ÇáĞí ÓíÙåÑ ÚäÏ ãæÊ ÇááÇÚÈ
    public TextMeshProUGUI gameOverText; // ÇáäÕ "Game Over" ÏÇÎá ÇáÜ Panel
    public Button restartButton; // ÒÑ ÅÚÇÏÉ ÇááÚÈÉ ÏÇÎá ÇáÜ Panel

    private void Start()
    {
        currentHealth = maxHealth; // ÈÏÁ ÇááÚÈÉ ÈÕÍÉ ßÇãáÉ
        gameOverPanel.SetActive(false); // ÇáÊÃßÏ ãä Ãä ÇáÜ Panel ÛíÑ ãİÚá İí ÇáÈÏÇíÉ
    }

    public void ChangeHealth(float value)
    {
        // ÊÚÏíá ÇáÕÍÉ ãÚ ÇáÊÃßÏ ãä ÃäåÇ Öãä ÇáäØÇŞ Èíä 0 æ maxHealth
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        Debug.Log("Current Health: " + currentHealth + " / " + maxHealth);

        UpdateHealthUI(); // ÊÍÏíË æÇÌåÉ ÇáãÓÊÎÏã ÈÚÏ ßá ÊÚÏíá ááÕÍÉ

        if (currentHealth <= 0)
        {
            Die(); // ÇÓÊÏÚÇÁ ÏÇáÉ ÇáãæÊ ÚäÏ ÇäÎİÇÖ ÇáÕÍÉ ááÕİÑ
        }
    }

    private void UpdateHealthUI()
    {
        // ÊÍÏíË æÇÌåÉ ÇáãÓÊÎÏã æİŞğÇ ááäæÚ (ÚÏæ Ãæ áÇÚÈ)
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
        // ÇáÈÍË Úä ÔÑíØ ÇáÕÍÉ İí Canvas ááÚÏæ æÊÍÏíËå
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
        // ÇáÈÍË Úä ÚäÇÕÑ æÇÌåÉ ÇáãÓÊÎÏã (UI) ááÇÚÈ æÊÍÏíËåÇ
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
            // ãäØŞ ÅäåÇÁ ÇááÚÈÉ ÚäÏ ãæÊ ÇááÇÚÈ
            Debug.Log("Game Over");
            gameOverText.text = "Game Over"; // ÚÑÖ ÇáäÕ "Game Over"
            gameOverPanel.SetActive(true); // ÊİÚíá ÇáÜ Panel áÚÑÖå
            restartButton.gameObject.SetActive(true); // ÊİÚíá ÒÑ ÅÚÇÏÉ ÇááÚÈÉ
            restartButton.onClick.AddListener(RestartGame); // ÑÈØ ÒÑ ÅÚÇÏÉ ÇááÚÈÉ ÈÇáÏÇáÉ RestartGame
        }
        else if (transform.CompareTag("Enemy"))
        {
            // ÒíÇÏÉ ÇáäŞÇØ ÚäÏ ãæÊ ÇáÚÏæ æÊÏãíÑå
            LevelManager.instance.score += killScore;
            Destroy(gameObject);

            // ÅäÔÇÁ ÊÃËíÑ ÌÒÆí ÚäÏ ãæÊ ÇáÚÏæ
            if (LevelManager.instance.particals.Length > 2)
            {
                Instantiate(LevelManager.instance.particals[2], transform.position, transform.rotation);
            }
        }
    }

    public void RestartGame()
    {
        // ÅÚÇÏÉ ÊÍãíá ÇáãÔåÏ ÇáÍÇáí
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

