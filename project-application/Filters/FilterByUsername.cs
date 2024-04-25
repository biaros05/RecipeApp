namespace filtering;
using users;
// this class will filter by username
public class FilterByUsername
{
    private readonly List<User> Users;
    public FilterByUsername(List<User> users)
    {
        if (users == null)
        {
            throw new InvalidOperationException("List of users cannot be null");
        }
        Users = users;
    }

    /// <summary>
    /// Filters the list of users created in constructor by the given username
    /// </summary>
    /// <param name="username">given user name to filter by</param>
    /// <returns>ONE filtered user object that matches</returns>
    public User FilterUsers(string username)
    {
        var filteredUser = Users
            .Where(user => user.Username == username)
            .SingleOrDefault();
        return filteredUser!; // returns null if user is now found
    }
}