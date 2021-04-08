using System;
using System.Collections.Generic;
using System.Text;


public class AriesPrimaryKeyAttribute : Attribute
{
    public readonly string KeyName;
    public AriesPrimaryKeyAttribute(string key)
    {
        KeyName = key;
    }
}

