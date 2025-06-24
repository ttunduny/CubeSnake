using UnityEngine;
public class MenuHolder : MonoBehaviour
{

    public GameObject mainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        if (mainMenu != null)
        {
            GameManager.gameManagerInstance.startGame();
            mainMenu.SetActive(false);
        }
    }
}
