﻿using GraphQL.Types;
using MyHotel.Entities;
using MyHotel.Models;

namespace MyHotel.GraphQL.Types
{
    public class GuestType : ObjectGraphType<GuestModel>
    {
        public GuestType()
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.RegisterDate);
        }
    }
}
