namespace DominioSerena
{
    public class ApiValidationProblem
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
