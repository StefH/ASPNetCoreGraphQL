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
using System.Security.Claims;

namespace MyHotel.GraphQL
{
    public class MyHotelQuery : ObjectGraphType
    {
        private readonly IAutoMapperPropertyNameResolver _autoMapperPropertyNameResolver;

        private ICollection<QueryArgumentInfo> PopulateQueryArgumentInfoList(Type type, Type parentType = null, string graphParentName = "", string parentEntityPath = "")
        {
            var list = new List<QueryArgumentInfo>();
            if (!(Activator.CreateInstance(type) is IComplexGraphType instance))
            {
                return list;
            }

            instance.Fields.ToList().ForEach(ft =>
            {
                Type graphType = ft.Type.GraphType();
                Type parentModel = parentType?.ModelType();

                string resolvedParentPath = _autoMapperPropertyNameResolver.Resolve(parentModel, graphParentName);

                string name = $"{graphParentName}{ft.Name}";
                string entityPath = !string.IsNullOrEmpty(parentEntityPath) ? $"{parentEntityPath}.{resolvedParentPath}" : resolvedParentPath;

                if (graphType.IsObjectGraphType())
                {
                    list.AddRange(PopulateQueryArgumentInfoList(graphType, type, name, entityPath));
                }
                else
                {
                    Type thisModel = type.ModelType();
                    string resolvedPropertyName = _autoMapperPropertyNameResolver.Resolve(thisModel, ft.Name);

                    list.Add(new QueryArgumentInfo
                    {
                        QueryArgument = new QueryArgument(graphType) { Name = name },
                        EntityPropertyPath = !string.IsNullOrEmpty(entityPath) ? $"{entityPath}.{resolvedPropertyName}" : resolvedPropertyName
                    });
                }
            });

            return list;
        }

        public MyHotelQuery(ReservationRepository reservationRepository, IAutoMapperPropertyNameResolver autoMapperPropertyNameResolver, IMapper mapper)
        {
            _autoMapperPropertyNameResolver = autoMapperPropertyNameResolver;

            var roomQueryArgumentList = PopulateQueryArgumentInfoList(typeof(RoomType));
            Field<ListGraphType<RoomType>>("rooms",
                arguments: new QueryArguments(roomQueryArgumentList.Select(q => q.QueryArgument)),
                resolve: context => reservationRepository
                    .GetRoomsQuery()
                    .ApplyQueryArguments(roomQueryArgumentList, context.Arguments)
                    .ProjectTo<RoomModel>(mapper.ConfigurationProvider)
                    .ToList()
            );

            var flatRoomQueryArgumentList = PopulateQueryArgumentInfoList(typeof(FlatRoomType));
            Field<ListGraphType<FlatRoomType>>("flatrooms",
                arguments: new QueryArguments(flatRoomQueryArgumentList.Select(q => q.QueryArgument)),
                resolve: context => reservationRepository
                    .GetRoomsQuery()
                    .ApplyQueryArguments(flatRoomQueryArgumentList, context.Arguments)
                    .ProjectTo<FlatRoomModel>(mapper.ConfigurationProvider)
                    .ToList()
            );

            var reservationQueryArgumentList = PopulateQueryArgumentInfoList(typeof(ReservationType));
            Field<ListGraphType<ReservationType>>("reservations",
                arguments: new QueryArguments(reservationQueryArgumentList.Select(q => q.QueryArgument)),
                resolve: context => reservationRepository
                    .GetReservationsQuery()
                    .ApplyQueryArguments(reservationQueryArgumentList, context.Arguments)
                    .ProjectTo<ReservationModel>(mapper.ConfigurationProvider)
                    .ToList()
            );

            Field<ListGraphType<ReservationType>>("reservations2",
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
