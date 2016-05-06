using System;
using System.Collections.ObjectModel;
using System.Linq;

public class Item
{
    public Guid Id { get; set; }
    public string Value { get; set; }
}

public class Library
{
    public ObservableCollection<Item> Items = new ObservableCollection<Item>();

    public void Add(string value)
    {
        Items.Add(new Item
        {
            Id = Guid.NewGuid(),
            Value = value
        });
    }

    public void Remove(Guid guid)
    {
        if (Items.Any(a => a.Id == guid))
        {
            Items.Remove(Items.First(s => s.Id == guid));
        }
    }
}
