namespace API.Contract.DTOs.Business.Import
{
    public class ImportResultResponse
    {
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<ImportRowError> Errors { get; set; } = [];

        public void AddError(int row, string message)
        {
            Errors.Add(new ImportRowError
            {
                Row = row,
                Message = message
            });
            FailedCount++;
        }
    }

    public class ImportRowError
    {
        public int Row { get; set; }
        public string Message { get; set; } = null!;
    }
}
