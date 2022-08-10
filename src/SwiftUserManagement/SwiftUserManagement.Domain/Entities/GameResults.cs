namespace SwiftUserManagement.Domain.Entities
{
    // Entity which will hold the result from the game
    public class GameResults
    {
        public string result1 { get; set; }
        public string result2 { get; set; }

        public GameResults(string result1, string result2)
        {
            this.result1 = result1;
            this.result2 = result2;
        }
    }
}
