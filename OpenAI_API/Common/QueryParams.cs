using System.Runtime.Serialization;

namespace OpenAI_API.Common
{
    /// <summary>
    /// Represents the query parameters for a list request.
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 20.
        /// </summary>
        public int Limit { get; set; } = 20;

        /// <summary>
        /// Sort order by the <c>created_at</c> of the objects. Options are <c>asc</c> for ascending order and
        /// <c>desc</c> for descending order. The default is <c>desc</c>.
        /// </summary>
        public OrderMode Order { get; set; } = OrderMode.Desc;

        /// <summary>
        /// The cursor for use in pagination. <c>after</c> is an object ID that defines your place in the list. For
        /// instance, if you make a list request and receive 100 objects, ending with obj_foo, your subsequent call can
        /// include after=obj_foo in order to fetch the next page of the list.
        /// </summary>
        public string After { get; set; }

        /// <summary>
        /// The cursor for use in pagination. <c>before</c> is an object ID that defines your place in the list. For
        /// instance, if you make a list request and receive 100 objects, starting with obj_bar, your subsequent call can
        /// include before=obj_bar in order to fetch the previous page of the list.
        /// </summary>
        public string Before { get; set; }

        /// <summary>
        /// Converts the query parameters to a query string.
        /// </summary>
        /// 
        /// <returns>
        /// The query string.
        /// </returns>
        public override string ToString()
        {
            var queryString = $"?limit={Limit}&order={Order.ToString().ToLower()}";

            if (!string.IsNullOrEmpty(After))
            {
                queryString += $"&after={After}";
            }

            if (!string.IsNullOrEmpty(Before))
            {
                queryString += $"&before={Before}";
            }

            return queryString;
        }
    }

    /// <summary>
    /// Enumerates the available sort modes.
    /// </summary>
    public enum OrderMode
    {
        /// <summary>
        /// Sort in ascending order.
        /// </summary>
        Asc,

        /// <summary>
        /// Sort in descending order.
        /// </summary>
        Desc
    }
}