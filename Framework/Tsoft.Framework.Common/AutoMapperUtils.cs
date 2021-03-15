using AutoMapper;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;

namespace Tsoft.Framework.Common
{
    public static class AutoMapperUtils
    {
        private static IMapper GetMapper<TSource, TDestination>()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddCollectionMappers();
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                cfg.CreateMap<TSource, TDestination>(MemberList.None);
            });

            IMapper mapper = new Mapper(config);
            return mapper;
        }
        private static IMapper GetMapper<TSource, TDestination>(string IdString)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddCollectionMappers();
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                cfg.CreateMap<TSource, TDestination>(MemberList.None).ForSourceMember(IdString, s => s.DoNotValidate()).ForMember(IdString, s => s.Ignore());

            });

            IMapper mapper = new Mapper(config);
            return mapper;
        }

        private static IMapper GetMapper<TSource1, TDestination1, TSource2, TDestination2>()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddCollectionMappers();
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                cfg.CreateMap<TSource1, TDestination1>(MemberList.None);
                cfg.CreateMap<TSource2, TDestination2>(MemberList.None);
            });

            IMapper mapper = new Mapper(config);
            return mapper;
        }
        #region Single
        public static TDestination AutoMap<TSource, TDestination>(TSource source)
        {
            var mapper = GetMapper<TSource, TDestination>();
            TDestination dest = mapper.Map<TDestination>(source);
            return dest;
        }

        public static TDestination1 AutoMap<TSource1, TDestination1, TSource2, TDestination2>(TSource1 source)
        {
            var mapper = GetMapper<TSource1, TDestination1, TSource2, TDestination2>();
            TDestination1 dest = mapper.Map<TDestination1>(source);
            return dest;
        }


        public static TDestination1 AutoMap<TSource1, TDestination1, TSource2, TDestination2>(TSource1 source, TDestination1 dest)
        {
            var mapper = GetMapper<TSource1, TDestination1, TSource2, TDestination2>();
            dest = mapper.Map(source, dest);
            return dest;
        }

        public static TDestination AutoMap<TSource, TDestination>(TSource source, string IdString)
        {
            var mapper = GetMapper<TSource, TDestination>(IdString);
            TDestination dest = mapper.Map<TDestination>(source);
            return dest;
        }
        public static TDestination AutoMap<TSource, TDestination>(TSource source, TDestination dest)
        {
            var mapper = GetMapper<TSource, TDestination>();
            dest = mapper.Map(source, dest);
            return dest;
        }

        public static TDestination AutoMap<TSource, TDestination>(TSource source, TDestination dest, string IdString)
        {
            var mapper = GetMapper<TSource, TDestination>(IdString);
            dest = mapper.Map(source, dest);
            return dest;
        }
        #endregion
        #region List
        public static List<TDestination> AutoMap<TSource, TDestination>(List<TSource> source)
        {
            var mapper = GetMapper<TSource, TDestination>();
            List<TDestination> dest = mapper.Map<List<TDestination>>(source);
            return dest;
        }

        public static List<TDestination> AutoMap<TSource, TDestination>(List<TSource> source, string IdString)
        {
            var mapper = GetMapper<TSource, TDestination>(IdString);
            List<TDestination> dest = mapper.Map<List<TDestination>>(source);
            return dest;
        }
        public static List<TDestination> AutoMap<TSource, TDestination>(List<TSource> source, List<TDestination> dest)
        {
            var mapper = GetMapper<TSource, TDestination>();
            dest = mapper.Map(source, dest);
            return dest;
        }

        public static List<TDestination> AutoMap<TSource, TDestination>(List<TSource> source, List<TDestination> dest, string IdString)
        {
            var mapper = GetMapper<TSource, TDestination>(IdString);
            dest = mapper.Map(source, dest);
            return dest;
        }
        #endregion

    }
}
