namespace Stove
{
    /// <summary>
    ///     Interface to get a user identifier.
    /// </summary>
    public interface IUserIdentifier
    {
        /// <summary>
        ///     Id of the user.
        /// </summary>
        long UserId { get; }
    }
}
