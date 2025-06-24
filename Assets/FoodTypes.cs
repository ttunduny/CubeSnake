using UnityEngine;

public class FoodTypes : MonoBehaviour
{
    public Color[] foodColors;

    private Material mat;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get Renderer
        Renderer myRenderer = GetComponent<Renderer>();
        //Check if you have a valid Renderer
        if(myRenderer==null || myRenderer.sharedMaterial == null)
        {
            Debug.Log("Error Retrieving Renderer");
            return;
        }
        //Create New Instance of material to make it unique for each food
        mat = new Material(myRenderer.sharedMaterial);

        int max_colors = foodColors.Length;

        int colorIndex = Random.Range(0, max_colors);

        mat.color = foodColors[colorIndex];

        myRenderer.material = mat;
        
    }

    


}
