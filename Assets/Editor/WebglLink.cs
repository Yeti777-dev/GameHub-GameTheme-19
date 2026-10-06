using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class WebglLink 
{
    // Start is called before the first frame update

    static WebglLink()
    {

        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        //PlayerSettings.WebGL.wasmStreaming = true;
        
    }

}
