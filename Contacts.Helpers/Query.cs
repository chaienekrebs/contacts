using System.Reflection;

namespace Contacts.Helpers
{
    public static class Query
    {
        public static IQueryable<TEntity> Filtrar<TEntity>(this IQueryable<TEntity> consulta, int limite, int pular) where TEntity : class
        {
            if (limite > 0)
            {
                consulta = consulta.Skip(pular).Take(limite);
            }

            return consulta;
        }

        public static IQueryable<TEntity> FiltrarPaginado<TEntity>(this IQueryable<TEntity> consulta, int start, int limit, string sort = "", string direction = "") where TEntity : class
        {

            sort = (sort ?? "");
            direction = (direction ?? "asc");

            var properties = typeof(TEntity).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(sort.ToLower()))
                {
                    prop = item;
                    break;
                }
            }
            var campoOrdem = prop;


            if (campoOrdem != null)
            {

                if (direction == "asc")
                {
                    consulta = consulta.OrderBy(campoOrdem.GetValue).AsQueryable().Skip(start).Take(limit);
                }
                else
                {
                    consulta = consulta.OrderByDescending(campoOrdem.GetValue).AsQueryable().Skip(start).Take(limit);
                }
            }
            else
            {

                consulta = consulta.AsQueryable().Skip(start).Take(limit);
            }

            return consulta;
        }

    }
}
