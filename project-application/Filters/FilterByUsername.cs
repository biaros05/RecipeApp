namespace filtering;
using users;
// this class will filter by ingredients
public class FilterByUsername
{
    private readonly List<User> Users;
    public FilterByUsername(List<User> users)
    {
        if (users == null || users.Count == 0)
        {
            throw new InvalidOperationException("List of users cannot be null or empty");
        }
        Users = users; 
    }

    public User FilterUsers(string username)
    {
        var filteredUser = Users
            .Where(user => user.Username == username)
            .SingleOrDefault();
        return filteredUser!; // returns null if user is now found
    }
}