namespace LSCoder.CodeJam.ConsoleProcessor
{
    public class ProblemInputFile
    {
        #region Constructors

        public ProblemInputFile(int id, string name, string path)
        {
            Id = id;
            Name = name;
            Path = path;
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        #endregion
    }
}
