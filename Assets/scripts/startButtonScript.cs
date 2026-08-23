using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
        spawnSvin.currentSvinCount = 0;
    }
}
