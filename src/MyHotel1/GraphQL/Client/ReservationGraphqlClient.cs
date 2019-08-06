﻿using GraphQL.Client;
using GraphQL.Common.Request;
using MyHotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotel.GraphQL.Client
{
    /*This class is created to use GraphQl.Client library*/
    public class ReservationGraphqlClient
    {
        private readonly GraphQLClient _client;

        public ReservationGraphqlClient(GraphQLClient client)
        {
            _client = client;
        }

        public async Task<List<Reservation>> GetReservationsAsync()
        {
            var query = new GraphQLRequest
            {
                Query = @"
query reservation {
  reservations {
    checkinDate
    guest  {
      name
    }
    room {
      name,
      number
    }
  }
}
"
            };

            var response = await _client.PostAsync(query);
            if (response.Errors != null && response.Errors.Any())
            {
                throw new ApplicationException(response.Errors[0].Message);
            }

            var reservations = response.GetDataFieldAs<List<Reservation>>("reservations");
            return reservations;
        }
    }
}
