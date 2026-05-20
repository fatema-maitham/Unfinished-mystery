using UnityEngine;
using System.IO;

public class PortraitTaker : MonoBehaviour
{
    public Camera portraitCamera;
    public int resWidth = 512;
    public int resHeight = 512;

    // This attribute adds the function to the Inspector component's right-click menu
    [ContextMenu("Take Portrait Photo")]
    public void TakeSnapshot()
    {
        if (portraitCamera == null)
        {
            Debug.LogError("Please assign the Portrait Camera in the inspector!");
            return;
        }

        // Create a temporary Render Texture in memory
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        portraitCamera.targetTexture = rt;

        // Render the camera's view manually
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
        portraitCamera.Render();

        // Read the pixels from the active render texture
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        // Clean up the texture links so the camera goes back to normal
        portraitCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        // Encode the texture into a PNG byte array
        byte[] bytes = screenShot.EncodeToPNG();

        // Generate a filename with a unique timestamp
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = Path.Combine(Application.dataPath, $"Character_Portrait_{timestamp}.png");

        // Save to your computer
        File.WriteAllBytes(filename, bytes);

        Debug.Log($"Portrait successfully saved to: {filename}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // Makes the image appear in your project window instantly
#endif
    }
}