using UnityEditor;

class Build {

    static void PerformBuild()
    {
        string[] scenes = { "Assets/Scene/Main.unity" };
        BuildPipeline.BuildPlayer(scenes, "Scratch/WinRT", BuildTarget.WSAPlayer, BuildOptions.None);
    }
}
