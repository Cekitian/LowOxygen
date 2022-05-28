using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;

public class ModsManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textBox;
    [Space]
    [Header("Adventure Display")]
    [SerializeField]
    private TextMeshProUGUI adventureName;
    [SerializeField]
    private TextMeshProUGUI adventureDescription;


    private bool writingInTextBox;
    private float detectNextInput = 0;
    private string modDirectoryName = "/Mods";
    private string modDirectoryPath;

    private int currentShownIndex = 0;
    private string[] directories;
    private bool lowerCaps = false;
    
    private void Awake()
    {
        CheckForMods();
        if(directories.Length > 0)
        CycleMods(0);
    }
    private void Update()
    {
        if(writingInTextBox)
        {
            if (Input.anyKeyDown)
                foreach (KeyCode x in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(x) && !x.ToString().Contains("Mouse"))
                        switch (x)
                        {
                            case KeyCode.Backspace:
                                textBox.text = textBox.text.Substring(0, textBox.text.Length - 1);
                                break;
                            case KeyCode.Space:
                                textBox.text += " ";
                                break;
                            default:
                                Debug.Log((char)x);
                               // if ((char)x >= 'A' && (char)x <= 'Z')
                                textBox.text += x.ToString();
                                break;
                        }

                }
        }
        if(Input.GetMouseButtonDown(0) && Time.time >= detectNextInput)
        {
            writingInTextBox = false;
        }
    }
    public void PlayWorld()
    {
        ModLoadedManager.Instance.SetDataPath(directories[currentShownIndex]);
        SceneManager.LoadScene(3);
    }
    public void EditWorld()
    {
        ModLoadedManager.Instance.SetDataPath(directories[currentShownIndex]);
        SceneManager.LoadScene(2);
    }
    public void StartWriting()
    {
        writingInTextBox = true;
        detectNextInput = Time.time + 0.1f;
    }
    public void CreateWorld()
    {
        string newWorldFolder = modDirectoryPath + "/" + textBox.text.ToString();

        if(!Directory.Exists(newWorldFolder))
        {
            Directory.CreateDirectory(newWorldFolder);
            File.WriteAllText(newWorldFolder + "/"  + textBox.text.ToString() +  "_description"," ");
        }

        CheckForMods();
        CycleMods(0);

    }
    public void CycleMods(int cycleDir)
    {
        Debug.Log("Cycling mods");

        if(currentShownIndex + cycleDir >= directories.Length)
        {
            currentShownIndex = 0;

        }else if (currentShownIndex + cycleDir <= -1)
        {
            currentShownIndex = directories.Length - 1;
        }
        else
        {
            currentShownIndex += cycleDir;
        }

        adventureName.text = directories[currentShownIndex].Substring(directories[currentShownIndex].IndexOf(Convert.ToChar(92)) + 1);
        
        adventureDescription.text = File.ReadAllText(directories[currentShownIndex]+ "/"
            + directories[currentShownIndex].Substring(directories[currentShownIndex].IndexOf(Convert.ToChar(92)) + 1)
             + "_description");
        // to get the name of the directory
    }

    private void CheckForMods()
    {
        string modsPath = Application.persistentDataPath + modDirectoryName;
        if(Directory.Exists(modsPath))
        {
            Debug.Log("I've found the mods folder!");
        }
        else
        {
            Debug.Log("I've not found the mods folder! I've just made one!");
            Directory.CreateDirectory(modsPath);
        }

        modDirectoryPath = modsPath;
        directories = Directory.GetDirectories(modsPath);

    }

}
