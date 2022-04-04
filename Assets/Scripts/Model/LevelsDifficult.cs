using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Model
{
    public class LevelsDifficult: List<LevelDifficult>
    {
        public LevelDifficult GetLevel(int levelId) => this.LastOrDefault(l => l.LevelId <= levelId);
    }
}