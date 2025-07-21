namespace Task2.Models;

public class EnvironmentVariable
{
    public event Action<EnvironmentVariable>? VariableChanged;

    public EnvironmentVariable(string name, string value, string comment)
    {
        Name = name;
        Value = value;
        Comment = comment;
    }

    public string Name { get; set; }

    private string _value;
    public string Value 
    { 
        get => _value;
        set
        {
            if (_value == value)
                return;

            _value = value;
            VariableChanged?.Invoke(this);
        } 
    }

    public string Comment { get; set; }

    public void Deconstruct(out string name, out string value) 
        => (name, value) = (Name, Value);
}
