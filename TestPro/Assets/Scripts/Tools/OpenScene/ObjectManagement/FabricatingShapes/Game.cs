using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    public class Game : PersistableObject
    {
        private void Awake()
        {
            
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Level_1");
        }
    }
}
