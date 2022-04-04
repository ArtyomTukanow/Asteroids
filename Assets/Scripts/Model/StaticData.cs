using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Model
{
    public enum ModelName
    {
        levels,
    }
    
    public class StaticData
    {
        public LevelsDifficult LevelsDifficult { get; private set; }

        public void LoadModel()
        {
            foreach (var staticName in (ModelName[])Enum.GetValues(typeof(ModelName)))
                LoadAndParse(staticName);
        }

        private void LoadAndParse(ModelName name) => Parse(name, Resources.Load<TextAsset>("Model/" + name).text);

        private void Parse(ModelName name, string jsonString)
        {
            LevelsDifficult = name switch
            {
                ModelName.levels => JsonConvert.DeserializeObject<LevelsDifficult>(jsonString),
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };
        }
    }
}