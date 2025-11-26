using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using System.Collections;

public class SkrypciorV2 : MonoBehaviour
{
    private string folderPath;
    private List<string> mp3Files;

    [Header("Prefabrykaty")]
    public GameObject buttonPrefab;
    public Transform gridParent;

    [Header("Audio")]
    public GameObject audioSourcePrefab; // Prefab AudioSource do instancjonowania
    public AudioMixer audioMixer; // Dodaj referencjê do Audio Mixer
    private bool isEarrapeActive = false; // Zmienna do œledzenia stanu earrape

    [Header("UI")]
    public GameObject editWindow;
    public GameObject panelToDeactivate;
    public GameObject przyciskEarrape;
    public GameObject przyciskUstawienia;
    public Button openOptionsButton;
    public Button earrapeButton; // Przycisk earrape
    public Button refreshButton; // Przycisk odœwie¿ania kafelków

    private GameObject selectedButton;
    private string selectedFilePath;

    private readonly string[] supportedImageFormats = { ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif", ".tif", ".tiff" };

    void Start()
    {
        folderPath = Path.Combine(Application.persistentDataPath, "SoundBoard");

        StartCoroutine(EnsureFolderCreated());
        EnsureFileNamesAreValid();
        RefreshUI();

        editWindow.SetActive(false);
        openOptionsButton.onClick.AddListener(OpenEditWindow);
        earrapeButton.onClick.AddListener(ToggleEarrape);
        refreshButton.onClick.AddListener(RefreshUI);
    }

    private IEnumerator EnsureFolderCreated()
    {
        try
        {
            CreateFolderIfNotExists(folderPath);
            mp3Files = LoadMP3Files(folderPath);
            CreateWindowsForMP3Files();
        }
        catch (Exception)
        {
        }

        yield return null;
    }

    private void CreateFolderIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private void EnsureFileNamesAreValid()
    {
        List<string> allFiles = Directory.GetFiles(folderPath, "*.mp3", SearchOption.TopDirectoryOnly).ToList();

        int maxIndex = 0;
        foreach (string file in allFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string[] parts = fileName.Split('~');
            if (parts.Length == 3 && int.TryParse(parts[0], out int index))
            {
                maxIndex = Mathf.Max(maxIndex, index);
            }
        }

        foreach (string file in allFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExtension = Path.GetExtension(file);

            string[] parts = fileName.Split('~');
            if (parts.Length != 3)
            {
                string newName;

                if (parts.Length == 1)
                {
                    maxIndex++;
                    newName = $"{maxIndex:D3}~{fileName}~100{fileExtension}";
                }
                else if (parts.Length == 2)
                {
                    maxIndex++;
                    newName = $"{maxIndex:D3}~{parts[1]}~100{fileExtension}";
                }
                else
                {
                    continue;
                }

                string newPath = Path.Combine(folderPath, newName);
                File.Move(file, newPath);
            }
        }
    }

    private List<string> LoadMP3Files(string path)
    {
        return Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly).ToList();
    }

    private void CreateWindowsForMP3Files()
    {
        foreach (string mp3File in mp3Files)
        {
            string fileName = Path.GetFileNameWithoutExtension(mp3File);
            string[] parts = fileName.Split('~');
            string displayName = parts[1];
            int volumePercentage = parts.Length == 3 && int.TryParse(parts[2], out int volume) ? volume : 100;

            GameObject newButton = Instantiate(buttonPrefab, gridParent);
            TMP_Text buttonText = newButton.transform.Find("Text").GetComponent<TMP_Text>();
            buttonText.text = displayName;

            Image buttonImage = newButton.transform.Find("Obrazek").GetComponent<Image>();
            string imagePath = FindMatchingImage(fileName);

            if (!string.IsNullOrEmpty(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                buttonImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() =>
            {
                PlayAudioWithVolume(mp3File, volumePercentage);
                AnimateButtonClick(newButton);
            });
        }
    }

    private string FindMatchingImage(string fileName)
    {
        foreach (string format in supportedImageFormats)
        {
            string imagePath = Path.Combine(folderPath, $"{fileName}{format}");
            if (File.Exists(imagePath))
                return imagePath;
        }
        return null;
    }

    private void PlayAudioWithVolume(string filePath, int volumePercentage)
    {
        if (File.Exists(filePath))
        {
            GameObject audioSourceObject = Instantiate(audioSourcePrefab);
            AudioSource currentAudioSource = audioSourceObject.GetComponent<AudioSource>();

            var audioClip = new WWW("file:///" + filePath);
            currentAudioSource.clip = audioClip.GetAudioClip(false, true, AudioType.MPEG);

            currentAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("EarrapeGroup")[0];

            float volume = volumePercentage / 100f;

            if (isEarrapeActive)
            {
                audioMixer.SetFloat("MasterVolume", 15f);
            }
            else
            {
                audioMixer.SetFloat("MasterVolume", 0f);
            }

            currentAudioSource.volume = volume;
            currentAudioSource.Play();

            Destroy(audioSourceObject, currentAudioSource.clip.length + 1f);
        }
    }

    private void AnimateButtonClick(GameObject button)
    {
        // Animacja skali przycisku
        button.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            button.transform.DOScale(1.0f, 0.2f).SetEase(Ease.InBounce);
        });

        // Animacja przechylenia (obrót wokó³ osi Z)
        button.transform.DORotate(new Vector3(0, 0, 10), 0.1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            button.transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.InOutSine);
        });
    }


    private void OpenEditWindow()
    {
        panelToDeactivate.SetActive(false);
        editWindow.SetActive(true);
        przyciskEarrape.SetActive(false);
        przyciskUstawienia.SetActive(false);
    }

    public void ToggleEarrape()
    {
        isEarrapeActive = !isEarrapeActive;
        float volume = isEarrapeActive ? 15.0f : 0.0f;
        audioMixer.SetFloat("Gowno", volume);
    }

    private void RefreshUI()
    {
        panelToDeactivate.SetActive(true);
        editWindow.SetActive(false);
        przyciskEarrape.SetActive(true);
        przyciskUstawienia.SetActive(true);

        foreach (Transform child in gridParent)
        {
            if (child.name != "GFHGHSDFGFUYRI" && child.name != "GUIREHGKJDSHKFDSHU")
            {
                Destroy(child.gameObject);
            }
        }

        mp3Files = LoadMP3Files(folderPath);
        CreateWindowsForMP3Files();
    }
}
