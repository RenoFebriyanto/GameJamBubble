using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialogBox; // Object dialog box
    public TextMeshProUGUI speakerNameText; // Text nama pembicara
    public TextMeshProUGUI dialogText;      // Text isi dialog

    [Header("Character Images")]
    public Image mainCharacterImage; // Gambar karakter utama
    public Image sideCharacterImage; // Gambar karakter sampingan
    public Image smallCharacterImage; // Gambar karakter sampingan kecil

    [Header("Dialog Data")]
    public StoryScene storyScene; // Referensi ke StoryScene

    [Header("Settings")]
    public float typingSpeed = 0.05f; // Kecepatan teks muncul
    public Color inactiveMainColor = new Color(0.54f, 0.54f, 0.54f); // Warna saat karakter utama tidak berbicara (8A8A8A)

    private Coroutine currentCoroutine;
    private bool isDialogActive = false;

    public static bool IsDialogActive { get; private set; } = false;

    private void Start()
    {
        // Menonaktifkan semua UI saat scene dimulai
        HideAllUI();
    }

    private void HideAllUI()
    {
        // Menyembunyikan semua elemen UI pada awal
        dialogBox.SetActive(false);
        mainCharacterImage.gameObject.SetActive(false);
        sideCharacterImage.gameObject.SetActive(false);
        smallCharacterImage.gameObject.SetActive(false);
    }

    public void StartDialog(StoryScene scene, System.Action onDialogEnd, bool isLastScene = false) // Tambahkan parameter isLastScene
    {
        if (isDialogActive) return;

        StartCoroutine(PlayDialog(scene, onDialogEnd, isLastScene)); // Pass parameter ke coroutine
    }

    private IEnumerator PlayDialog(StoryScene scene, System.Action onDialogEnd, bool isLastScene)
    {
        dialogBox.SetActive(true);
        mainCharacterImage.gameObject.SetActive(true);
        sideCharacterImage.gameObject.SetActive(true);
        smallCharacterImage.gameObject.SetActive(true);

        isDialogActive = true;

        foreach (var sentence in scene.sentences)
        {
            speakerNameText.text = sentence.speaker.speakerName;
            speakerNameText.color = sentence.speaker.textColor;
            UpdateCharacterImage(sentence.speaker);

            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(TypeSentence(sentence.text));

            float displayTime = sentence.displayTime > 0 ? sentence.displayTime : 2f;
            yield return new WaitForSeconds(displayTime);
        }

        HideAllUI();
        isDialogActive = false;
        onDialogEnd?.Invoke();

        if (isLastScene)
        {
            yield return new WaitForSeconds(1f); // Tunggu 1 detik
            SceneManager.LoadScene("MainMenu"); // Ganti dengan nama scene MainMenu
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void UpdateCharacterImage(Speaker speaker)
    {
        // Reset semua gambar karakter terlebih dahulu
        mainCharacterImage.color = inactiveMainColor;
        sideCharacterImage.gameObject.SetActive(false);
        smallCharacterImage.gameObject.SetActive(false); // Pastikan ini selalu disembunyikan

        // Cek apakah karakter ini adalah karakter utama
        if (speaker.isMainCharacter)
        {
            mainCharacterImage.sprite = speaker.characterImage;
            mainCharacterImage.color = Color.white; // Tampilkan karakter utama
        }
        else if (speaker.isSmallCharacter)  // Tambahkan pengecekan untuk karakter kecil
        {
            // Menampilkan karakter kecil pada UI
            smallCharacterImage.sprite = speaker.characterImage;
            smallCharacterImage.gameObject.SetActive(true); // Aktifkan gambar karakter kecil
        }
        else
        {
            // Jika bukan karakter utama atau kecil, tampilkan karakter sampingan
            sideCharacterImage.sprite = speaker.characterImage;
            sideCharacterImage.gameObject.SetActive(true); // Aktifkan gambar karakter sampingan
        }
    }
}
