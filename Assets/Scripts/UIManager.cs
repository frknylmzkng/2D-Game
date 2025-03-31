using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Oyun arayüzü yönetimi
public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    // Olay dinleyicilerini etkinleþtir
    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamage);
        CharacterEvents.characterHealed += (CharacterHealed);
    }

    // Olay dinleyicilerini devre dýþý býrak
    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamage);
        CharacterEvents.characterHealed -= (CharacterHealed);
    }

    // Karakter hasar adlýðýnda tetiklenen olay
    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        // Karakterin konumunda metin oluþtur
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        // Prefab kullanarak hasar metnini oluþtur
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }

    // Karakter iyileþtiðinde tetiklenen olay
    public void CharacterHealed(GameObject character, int healthRestored)
    {
        // Karakterin konumunda metin oluþtur
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        // Prefab kullanarak iyileþme metnini oluþtur
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healthRestored.ToString();
    }

    // Oyundan çýkýþ iþlemi için kullanýcý giriþini iþleyen fonksiyon
    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            // Oyunun çalýþtýðý platforma göre uygun çýkýþ iþlemi yapýlýr
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
