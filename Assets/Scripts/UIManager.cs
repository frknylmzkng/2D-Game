using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Oyun aray�z� y�netimi
public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    // Olay dinleyicilerini etkinle�tir
    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamage);
        CharacterEvents.characterHealed += (CharacterHealed);
    }

    // Olay dinleyicilerini devre d��� b�rak
    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamage);
        CharacterEvents.characterHealed -= (CharacterHealed);
    }

    // Karakter hasar adl���nda tetiklenen olay
    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        // Karakterin konumunda metin olu�tur
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        // Prefab kullanarak hasar metnini olu�tur
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }

    // Karakter iyile�ti�inde tetiklenen olay
    public void CharacterHealed(GameObject character, int healthRestored)
    {
        // Karakterin konumunda metin olu�tur
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        // Prefab kullanarak iyile�me metnini olu�tur
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healthRestored.ToString();
    }

    // Oyundan ��k�� i�lemi i�in kullan�c� giri�ini i�leyen fonksiyon
    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            // Oyunun �al��t��� platforma g�re uygun ��k�� i�lemi yap�l�r
            #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_STANDALONE)
                Application.Quit();
            #elif (UNITY_WEBGL)
                SceneManager.LoadScene("QuitScene")
            #endif
        }
    }
}
