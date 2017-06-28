using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestMove {

	[Test]
	public void EditorTest() {
        InitializeScene();

        //Arrange
        var gameObject = GameObject.FindGameObjectWithTag("Player");

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}

    private void InitializeScene()
    {
        PlaySpaceManager playSpaceManager = GameObject.FindGameObjectWithTag("Spatial Processing").GetComponent<PlaySpaceManager>();

        playSpaceManager.finishScanning();

        while(!playSpaceManager.finishedScanning())
        {
            // Wait..
        }
    }
}
