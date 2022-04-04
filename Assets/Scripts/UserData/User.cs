namespace UserData
{
    public class User
    {
        public UserPlayer Player { get; private set; }
        public UserLevel Level { get; private set; }
        
        public void Init()
        {
            Player = new UserPlayer();
            Level = new UserLevel();
            
            Player.Init();
            Level.Init();
        }

        public void OnDestroy()
        {
            Player.OnDestroy();
            Level.OnDestroy();
        }

        public void PlayAgain()
        {
            Player.Score = 0;
            Level.Restart();
        }
    }
}