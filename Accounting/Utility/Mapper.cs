using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Configuration.Conventions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Accounting.Program;

namespace Accounting.Utility
{
    internal class Mapper
    {

        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source, Action<IMappingExpression<TSource, TDestination>> callback)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                IMappingExpression<TSource, TDestination> mappingExpression = cfg.CreateMap<TSource, TDestination>();
                callback(mappingExpression);
            });

            IMapper mapper = config.CreateMapper();

            IEnumerable<TDestination> destination = source.Select(x => mapper.Map<TSource, TDestination>(x));

            return destination;
        }


        public static TDestination Map<TSource, TDestination>(TSource source, Action<IMappingExpression<TSource, TDestination>> callback)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                IMappingExpression<TSource, TDestination> mappingExpression = cfg.CreateMap<TSource, TDestination>();
                callback(mappingExpression);
            });

            IMapper mapper = config.CreateMapper();

            var destination = mapper.Map<TSource, TDestination>(source);

            return destination;
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                IMappingExpression<TSource, TDestination> mappingExpression = cfg.CreateMap<TSource, TDestination>();
            });

            IMapper mapper = config.CreateMapper();

            IEnumerable<TDestination> destination = source.Select(x => mapper.Map<TSource, TDestination>(x));

            return destination;
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                IMappingExpression<TSource, TDestination> mappingExpression = cfg.CreateMap<TSource, TDestination>();
            });

            IMapper mapper = config.CreateMapper();

            var destination = mapper.Map<TSource, TDestination>(source);

            return destination;
        }


    }
}
