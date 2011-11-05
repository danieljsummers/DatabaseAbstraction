namespace DatabaseAbstraction.Models
{
    /// <summary>
    /// The list of potential query fragments that can be implemented, in the order they will be substituted when
    /// assembling the query fragments
    /// </summary>
    public enum QueryFragmentType
    {
        Select,
        Insert,
        Update,
        Delete,
        From,
        Where,
        OrderBy
    }
}