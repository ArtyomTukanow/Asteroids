using ECS.Component;

namespace World.Events
{
    /// <summary>
    /// Событие, сообщающее, что космический объект уничтожен
    /// </summary>
    public class DestroySEvent : IECSComponent
    {
        public int Score = 0;
    }
}