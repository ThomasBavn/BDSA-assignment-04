namespace SeperationGood;

public class Company
{
    public string _companyName;

    public IEnumerable<Customer> _customers;

    public Company(string comanyName)
    {
        _companyName = comanyName;
        _customers = new List<Customer>();
    }

    public void AddCustomer(string name, string email)
    {   
        _customers.Add(new Customer(name, email));
    }
}