using Newtonsoft.Json;

namespace OsosOracle.Framework.Utilities.Parsers
{
    /// <summary>
    /// kaynak classı sonuç türüne çevirir
    /// </summary>
    /// <typeparam name="TSource">Kaynak Class</typeparam>
    /// <typeparam name="TResult">Sonuç Class</typeparam>
    public static class ClassConverter<TSource, TResult>
    {

        public static TResult Convert(TSource item)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject(item, jsonSerializerSettings));
        }


        public static TResult ConvertWithMapper(TSource item)
        {
            //var jsonSerializerSettings = new JsonSerializerSettings
            //{
            //    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};



            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TSource, TResult>();
            });

            var objectToSerialize = AutoMapper.Mapper.Map<TResult>(item);

            return objectToSerialize;
            //return JsonConvert.DeserializeObject<TResult>(JsonConvert.SerializeObject(item, jsonSerializerSettings));
        }

        /*
         
         
        

         */


    }
}
