using ECS.Component;

namespace World.Component
{
    public static class AsteroidLevelUtils
    {
        public const float ASTEROID_DEF_SPEED = 0.01f;
        public const int MAX_VALUE = (int)AsteroidLevel.big;
        
        public static float MinSpeed(this AsteroidLevel level)
        {
            return (float)(MAX_VALUE - (int) level) / MAX_VALUE * ASTEROID_DEF_SPEED;
        }
        
        public static float MaxSpeed(this AsteroidLevel level)
        {
            return (MAX_VALUE - (int) level + 1) * ASTEROID_DEF_SPEED;
        }
    }
    
    public enum AsteroidLevel
    {
        none = 0,
        small = 1,
        medium = 2,
        big = 3,
    }
    
    /// <summary>
    /// Астрероид, имеющий определенный размер
    /// </summary>
    public class AsteroidComponent : IECSComponent
    {
        public AsteroidLevel Level;
    }
}