using AutoMapper;
using Utility.Negocio;

namespace Utility.Extensions;

public static class ListExtensions
{
    public static void Foreach<T>(this List<T> items, Action<T> action)
    {
        foreach (var item in items)
        {
            action?.Invoke(item);
        }
    }

    public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
    {
        var count = 0;

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (match(list[i]))
            {
                ++count;
                list.RemoveAt(i);
            }
        }

        return count;
    }

    /// <summary>
    /// Copia la lista de un destino a otro 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="to"></param>
    /// <param name="source"></param>
    /// <param name="deleteIfNotExists"></param>
    public static void CopyListEquivalentFrom<T>(this IList<T> to, IEnumerable<T> source, bool deleteIfNotExists = true) where T : IEquivalentComparer<T>
    {
        var mapper = new Mapper(new MapperConfiguration(
                e => e.CreateMap<T, T>()
                    .ForMember(p => p.Id, opt => opt.Ignore())
            )
        );

        if (deleteIfNotExists)
        {
            var deleted = to.Where(e => source.All(c => !e.IsEquivalentTo(c))).ToList();

            foreach (var noExiste in deleted)
            {
                to.Remove(noExiste);
            }
        }

        foreach (var itemSource in source)
        {
            var destination = to.FirstOrDefault(e => e.IsEquivalentTo(itemSource) && itemSource.Id != 0);
            if (destination != null)
            {
                mapper.Map(itemSource, destination);
            }
            else
            {
                to.Add(itemSource);
            }
        }
    }

    public static void CopyListFrom<T>(this IList<T> to, IEnumerable<T> source, bool deleteIfNotExists = true) where T : IEntity
    {
        var mapper = new Mapper(new MapperConfiguration(
                e => e.CreateMap<T, T>()
                    .ForMember(p => p.Id, opt => opt.Ignore())
            )
        );

        if (deleteIfNotExists)
        {
            var deleted = to.Where(e => source.All(c => c.Id != e.Id)).ToList();

            foreach (var noExiste in deleted)
            {
                to.Remove(noExiste);
            }
        }

        foreach (var itemSource in source)
        {
            var destination = to.FirstOrDefault(e => e.Id == itemSource.Id && itemSource.Id != 0);
            if (destination != null)
            {
                mapper.Map(itemSource, destination);
            }
            else
            {
                to.Add(itemSource);
            }
                
        }
    }

    public static T Copy<T>(this T from) where T : IEntity, new()
    {
        var mapper = new Mapper(new MapperConfiguration(
                e => e.CreateMap<T, T>()
                    .ForMember(p => p.Id, opt => opt.Ignore())
            )
        );

        var destination = new T();

        mapper.Map(from, destination);

        return destination;

    }
    public static void AddRange<T>(this IList<T> to, IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            to.Add(item);
        }
    }
         

    public static void AddRangeAt<T>(this IList<T> list, int index, IEnumerable<T> collection)
    {
        if (index >= list.Count)
        {
            list.AddRange(collection);
            return;
        }
            
        foreach (var item in collection)
        {
            list.Insert(index++, item);
        }
    }

    public static IList<T> ToAdjacentTree<T>(this IList<T> list, int parentId = 0, int level = 0) where T : class, ITree
    {
        var result = list.Where(e => e.ParentId == parentId)
            .OrderBy(e=>e.Order)
            .ToList();

        foreach (var parent in result)
        {
            var index = result.FindIndex(e => e.Id == parent.Id);
            var item = result[index];
            item.Level = level;
            var children = list.ToAdjacentTree(parent.Id, level + 1);
            item.IsLeaf = children.Count == 0;
            result.AddRangeAt(index + 1,children);
        }

        return result.ToList();
    }
}
