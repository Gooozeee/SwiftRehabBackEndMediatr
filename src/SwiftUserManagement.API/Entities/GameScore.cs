namespace SwiftUserManagement.API.Entities
{
    public class GameScore
    {
        public int GameScore_Id { get; set; }
        public int User_Id { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public string Explanation { get; set; }
    }
}
