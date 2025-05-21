using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
