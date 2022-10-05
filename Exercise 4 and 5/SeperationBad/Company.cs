
namespace SeperationBad;

public class Company
{

    public string _companyName;

    public List<string> customerNames;
    public List<string> customerEmails;


    public Company(string companyName)
    {
        _companyName = companyName;
        customerNames = new List<string>();
        customerEmails = new List<string>();
    }

    public AddCustomer(string name, string email)
    {
        customerNames.Add(name);
        customerEmails.Add(email);

    }

}