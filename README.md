# GraphQL.EntityFrameworkCore.DynamicLinq

Add EntityFramework Core Dynamic IQueryable support to GraphQL.


## NuGet
[![GraphQL.EntityFrameworkCore.DynamicLinq](https://buildstats.info/nuget/GraphQL.EntityFrameworkCore.DynamicLinq)](https://www.nuget.org/packages/GraphQL.EntityFrameworkCore.DynamicLinq)



# Information

With this project you can easily expose all properties from the EF Entities as searchable fields on the GraphQL query.

## Entity Example

``` c#
public class Room
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Number { get; set; }

    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    public RoomStatus Status { get; set; }

    public bool AllowedSmoking { get; set; }
}
```

## GraphQL

With this library you can use GraphQL queries like:


























# ASPNetCoreGraphQL
- Sample project based on <a href="https://fullstackmark.com/post/17/building-a-graphql-api-with-aspnet-core-2-and-entity-framework-core">the blog post</a> demonstrating how to build a GraphQL service powered by ASP.NET Core 2.2, Entity Framework Core and <a href="https://github.com/graphql-dotnet/graphql-dotnet" target="_blank">graphql-dotnet</a>.

- Sample project based on https://github.com/ebicoglu/AspNetCoreGraphQL-MyHotel

- Sample project based on https://github.com/tpeczek/Demo.Azure.Functions.GraphQL

- Sample project based on https://github.com/Edward-Zhou/AspNetCore/tree/master/GraphQL