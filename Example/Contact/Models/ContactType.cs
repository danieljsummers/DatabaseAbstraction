namespace DatabaseAbstraction.Contact.Models
{
    /// <summary>
    /// These correspond to the values in the R_CONTACT_TYPE table, and list the
    /// types of contact information tracked
    /// </summary>
    public enum ContactType
    {
        Home = 1,
        Business = 2,
        Cell = 3,
        Fax = 4
    }
}