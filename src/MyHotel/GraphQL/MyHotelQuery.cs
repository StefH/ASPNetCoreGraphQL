using AutoMapper;
using AutoMapper.QueryableExtensions;
using GraphQL;
using GraphQL.Types;
using MyHotel.AutoMapper;
using MyHotel.Entities;
using MyHotel.Extensions;
using MyHotel.GraphQL.Types;
using MyHotel.Models;
using MyHotel.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace MyHotel.GraphQL
{
    public class MyHotelQuery : ObjectGraphType
    {
        private readonly IAutoMapperPropertyNameResolver _autoMapperPropertyNameResolver;

        private void Loop(Type type, Type parentType, string graphParentName, IList<QueryArgumentInfo> list)
        {
            if (!(Activator.CreateInstance(type) is IComplexGraphType instance))
            {
                return;
            }

            instance.Fields.ToList().ForEach(ft =>
            {
                Type graphType = ft.Type.GraphType();
                if (graphType.IsObjectGraphType())
                {
                    Loop(graphType, type, ft.Name, list);
                }
                else
                {
                    Type parentModel = parentType?.ModelType();
                    string resolvedParentPrefix = _autoMapperPropertyNameResolver.Resolve(parentModel, graphParentName);

                    Type thisModel = type.ModelType();
                    string resolvedPropertyName = _autoMapperPropertyNameResolver.Resolve(thisModel, ft.Name);

                    list.Add(new QueryArgumentInfo
                    {
                        QueryArgument = new QueryArgument(graphType) { Name = $"{graphParentName.ToLowerInvariant()}{ft.Name}" },
                        EntityPropertyPath = parentType != null ? $"{resolvedParentPrefix}.{resolvedPropertyName}" : resolvedPropertyName
                    });
                }
            });
        }

        public MyHotelQuery(ReservationRepository reservationRepository, IAutoMapperPropertyNameResolver autoMapperPropertyNameResolver, IMapper mapper)
        {
            _autoMapperPropertyNameResolver = autoMapperPropertyNameResolver;
            var roomQueryArgumentList = new List<QueryArgumentInfo>();
            Loop(typeof(RoomType), null, "", roomQueryArgumentList);

            Field<ListGraphType<RoomType>>("rooms",
                arguments: new QueryArguments(roomQueryArgumentList.Select(q => q.QueryArgument)),
                resolve: context =>
                {
                    IQueryable query = reservationRepository.GetRoomsQuery().AsQueryable();

                    Console.WriteLine("rooms: context.Arguments = " + JsonConvert.SerializeObject(context.Arguments));

                    if (context.Arguments != null)
                    {
                        foreach (var argument in context.Arguments)
                        {
                            var info = roomQueryArgumentList.First(i => i.QueryArgument.Name == argument.Key);
                            query = query.Where($"np({info.EntityPropertyPath}) == {argument.Value}");
                        }
                    }

                    var list = query.ProjectTo<RoomModel>(mapper.ConfigurationProvider).ToList();
                    return list;
                }
            );

            Field<ListGraphType<ReservationType>>("reservations",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    },
                    new QueryArgument<DateGraphType>
                    {
                        Name = "checkinDate"
                    },
                    new QueryArgument<DateGraphType>
                    {
                        Name = "checkoutDate"
                    },
                    new QueryArgument<BooleanGraphType>
                    {
                        Name = "roomAllowedSmoking"
                    },
                    new QueryArgument<RoomStatusType>
                    {
                        Name = "roomStatus"
                    },
                    new QueryArgument<IntGraphType>
                    {
                        Name = "roomWindows"
                    },
                    new QueryArgument<IntGraphType>
                    {
                        Name = "roomBeds"
                    }
                }),
                resolve: context =>
                {
                    var query = reservationRepository.GetReservationsQuery().AsQueryable();

                    Console.WriteLine("context.Arguments = " + JsonConvert.SerializeObject(context.Arguments));

                    var user = (ClaimsPrincipal)context.UserContext;
                    var isUserAuthenticated = ((ClaimsIdentity)user.Identity).IsAuthenticated;

                    var reservationId = context.GetArgument<int?>("id");
                    if (reservationId.HasValue)
                    {
                        if (reservationId.Value <= 0)
                        {
                            context.Errors.Add(new ExecutionError("reservationId must be greater than zero!"));
                            return new List<Reservation>();
                        }

                        query = query.Where(r => r.Id == reservationId.Value);
                    }

                    var checkinDate = context.GetArgument<DateTime?>("checkinDate");
                    if (checkinDate.HasValue)
                    {
                        query = query.Where(r => r.CheckinDate.Date == checkinDate.Value.Date);
                    }

                    var checkoutDate = context.GetArgument<DateTime?>("checkoutDate");
                    if (checkoutDate.HasValue)
                    {
                        query = query.Where(r => r.CheckoutDate.Date >= checkoutDate.Value.Date);
                    }

                    var allowedSmoking = context.GetArgument<bool?>("roomAllowedSmoking");
                    if (allowedSmoking.HasValue)
                    {
                        query = query.Where(r => r.Room.AllowedSmoking == allowedSmoking.Value);
                    }

                    var roomStatus = context.GetArgument<RoomStatus?>("roomStatus");
                    if (roomStatus.HasValue)
                    {
                        query = query.Where(r => r.Room.Status == roomStatus.Value);
                    }

                    var roomWindows = context.GetArgument<int?>("roomWindows");
                    if (roomWindows.HasValue)
                    {
                        query = query.Where(r => r.Room.RoomDetail.Windows == roomWindows);
                    }

                    var roomBeds = context.GetArgument<int?>("roomBeds");
                    if (roomBeds.HasValue)
                    {
                        query = query.Where(r => r.Room.RoomDetail.Beds == roomBeds);
                    }

                    return query.ProjectTo<ReservationModel>(mapper.ConfigurationProvider).ToList();
                }
            );

        }
    }
}
