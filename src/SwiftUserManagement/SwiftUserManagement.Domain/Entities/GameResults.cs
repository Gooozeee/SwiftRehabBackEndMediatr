namespace SwiftUserManagement.Domain.Entities
{
    // Entity which will hold the result from the game
    public class GameResults
    {
        public int result1 { get; set; }
        public int result2 { get; set; }

        public GameResults(int result1, int result2)
        {
            this.result1 = result1;
            this.result2 = result2;
        }
    }
}
