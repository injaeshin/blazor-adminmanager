using System.Collections;
using AdminManager.Model;
using AdminManager.ModelView;
using Nelibur.ObjectMapper;

namespace AdminManager.Service;

public interface IMapperService
{
    TDestination Map<TSource, TDestination>(TSource source);
}

public class MapperService : IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return TinyMapper.Map<TDestination>(source);
    }
}

