using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScrollMaster2D.Controllers;

namespace ScrollMaster2D.Managers
{

    public class GameInitializer : MonoBehaviour
    {
        public ItemConfig[] itemConfigs;

        void Awake()
        {
            foreach (var config in itemConfigs)
            {
                ItemDatabase.AddItemConfig(config);
            }
        }
    }


}