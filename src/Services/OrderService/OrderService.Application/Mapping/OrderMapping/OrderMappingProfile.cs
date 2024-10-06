using System;
using AutoMapper;
using OrderService.Application.Features.Comments.CreateCommand;
using OrderService.Application.Features.Queries.ViewModels;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Application.Mapping.OrderMapping;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, CreateOrderCommand>()
            .ReverseMap();

        CreateMap<OrderItem, CreateOrderCommand.OrderItemDto>()
            .ReverseMap();
        
        CreateMap<Order, OrderDetailViewModel>()
            .ForMember(dest=> dest.City, opt=> opt.MapFrom(src=> src.Address.City))
            .ForMember(dest=> dest.Country, opt=> opt.MapFrom(src=> src.Address.Country))
            .ForMember(dest=> dest.Street, opt=> opt.MapFrom(src=> src.Address.Street))
            .ForMember(dest=> dest.Zipcode, opt=> opt.MapFrom(src=> src.Address.ZipCode))
            .ForMember(dest=> dest.Date, opt=> opt.MapFrom(src=> src.OrderDate))
            .ForMember(dest=> dest.OrderNumber, opt=> opt.MapFrom(src=> src.Id.ToString()))
            .ForMember(dest=> dest.Status, opt=> opt.MapFrom(src=> src.OrderStatus))
            .ForMember(dest=> dest.Total, opt=> opt.MapFrom(src=> src.OrderItems.Sum(x=> x.UnitPrice * x.Units)))
            .ReverseMap();

        CreateMap<OrderItem, OrderDetailViewModel.OrderItem>();
    }

}
