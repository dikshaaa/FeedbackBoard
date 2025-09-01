namespace FeedbackBoard.Domain.Enums
{
    /// <summary>
    /// Specifies the sorting criteria for feedback
    /// </summary>
    public enum SortBy
    {
        CreatedAt,
        Rating,
        Name
    }

    /// <summary>
    /// Specifies the sorting direction
    /// </summary>
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
