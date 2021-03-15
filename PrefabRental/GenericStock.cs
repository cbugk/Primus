using System;

namespace Primus.PrefabRental
{
    public class Stock
    {

    } 

    public class GenericStock<TEnum, TB> : Stock where TB : GenericBaseProduct<TEnum> where TEnum : Enum
    {
        
    }
}