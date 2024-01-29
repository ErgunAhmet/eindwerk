using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class AutoMapperExtensions
    {
        private static IMapper _mapper;

        /// <summary>
        /// Map a list object to TDestination
        /// </summary>
        /// <typeparam name="TDestination">The type of object you want mapped</typeparam>
        /// <param name="list">The list object to map</param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TDestination>(this IEnumerable<object> list)
        {
            return _mapper.Map<IEnumerable<TDestination>>(list);
        }

        /// <summary>
        /// Map an object
        /// </summary>
        /// <typeparam name="TDestination">The type of object you want mapped</typeparam>
        /// <param name="objectToMap"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(this object objectToMap)
        {
            return _mapper.Map<TDestination>(objectToMap);
        }

        public static void SetMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
