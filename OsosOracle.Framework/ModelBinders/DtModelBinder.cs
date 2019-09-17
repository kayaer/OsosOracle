using OsosOracle.Framework.SharedModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OsosOracle.Framework.ModelBinders
{
    //jqDataTable Model Binder
    public class DtModelBinder : System.Web.Mvc.IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            // Retrieve request data
            int draw = Convert.ToInt32(request["draw"]);
            int start = Convert.ToInt32(request["start"]);
            int length = Convert.ToInt32(request["length"]);

            // Search
            var search = new DtSearch
            {
                Value = request["search[value]"],
                Regex = Convert.ToBoolean(request["search[regex]"])
            };

            // Order
            var o = 0;
            var order = new List<DtOrder>();
            while (request["order[" + o + "][column]"] != null)
            {
                order.Add(new DtOrder()
                {
                    Column = Convert.ToInt32(request["order[" + o + "][column]"]),
                    Dir = request["order[" + o + "][dir]"]
                });
                o++;
            }
            // Columns
            var c = 0;
            var columns = new List<DtColumn>();
            while (request["columns[" + c + "][name]"] != null)
            {
                columns.Add(new DtColumn
                {
                    Data = request["columns[" + c + "][data]"],
                    Name = request["columns[" + c + "][name]"],
                    Orderable = Convert.ToBoolean(request["columns[" + c + "][orderable]"]),
                    Search = new DtSearch
                    {
                        Value = request["columns[" + c + "][search][value]"],
                        Regex = Convert.ToBoolean(request["columns[" + c + "][search][regex]"])
                    }
                });
                c++;
            }

            return new DtParameterModel
            {
                Draw = draw,
                Start = start,
                Length = length,
                Search = search,
                Order = order,
                Columns = columns
            };
        }
    }
}
