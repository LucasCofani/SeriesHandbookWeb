namespace SeriesHandbookShared.Models.TMDB
{
    public class ResponseWrapper<T>
    {
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
        public T Info { get; set; }

        public static ResponseWrapper<T> Ok(T res)
        {
            return new()
            {
                Info = res,
                ErrorMsg = null,
                Status = "Ok"
            };
        }
        public static ResponseWrapper<T> Error(string errorMsg)
        {
            return new()
            {
                Info = default(T),
                ErrorMsg = errorMsg,
                Status = "Error"
            };
        }
    }
}
