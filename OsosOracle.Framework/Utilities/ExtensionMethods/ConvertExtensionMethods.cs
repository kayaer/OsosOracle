using Newtonsoft.Json;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using OsosOracle.Framework.Utilities.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OsosOracle.Framework.Utilities.ExtensionMethods
{
    public static class ConvertExtensionMethods
    {
        public static int ToInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static bool ToBoolean(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static long ToLong(this object obj)
        {
            return Convert.ToInt64(obj);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<T> ToList<T>(this string value, char split = ',')
        {
            if (System.String.IsNullOrEmpty(value))
                return null;

            return (from veri in value.Split(split) where !System.String.IsNullOrEmpty(veri) select (T)Convert.ChangeType(veri, typeof(T))).ToList<T>();
        }

        public static List<T> List<T>(this T value)
        {
            return new List<T> { value };
        }

        /// <summary>
        /// İlk kayıt varsa getirir yoksa hata verir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="hataMesaji"></param>
        /// <returns></returns>
        public static T IlkKayit<T>(this List<T> values, string hataMesaji = "Kayıt bulunamadı")
        {
            var value = values.FirstOrDefault();

            if (value == null)
                throw new NotificationException(hataMesaji);
            return value;
        }

        public static List<TEntityEf> ConvertEfList<TEntity, TEntityEf>(this List<TEntity> values)
        {
            return ClassConverter<List<TEntity>, List<TEntityEf>>.Convert(values);
        }

        public static List<TEntityEf> ConvertEfList<TEntity, TEntityEf>(this TEntity value)
        {
            return ClassConverter<List<TEntity>, List<TEntityEf>>.Convert(value.List());
        }

        public static TEntityEf ConvertEf<TEntity, TEntityEf>(this TEntity value)
        {
            return ClassConverter<TEntity, TEntityEf>.Convert(value);
        }

        public static string ToString(this List<int> value, char split = ',')
        {
            var birlestirilenString = "";
            foreach (var deger in value)
            {
                birlestirilenString += deger + ",";
            }

            if (birlestirilenString.Length > 0)
                birlestirilenString = birlestirilenString.Remove(birlestirilenString.Length - 1, 1);

            return birlestirilenString;
        }


        /// <summary>
        /// newtonsoft ile objeyi json string yapar, selfloop kapalı, null ignorant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {



            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, Formatting.None, jsonSerializerSettings).Replace('"', '\'');

        }





        public static object Clone(this object obj)
        {
            return ClassConverter<object, object>.Convert(obj);

        }

        public static T Clone<T>(this object obj)
        {
            return (T)ClassConverter<object, object>.Convert(obj);
        }

        /// <summary>
        /// objeyi key=val&amp;key1=val1... formatına çevirir
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }



        /// <summary>
        /// upload edilen dosyayı byte array haline getirir
        /// </summary>
        /// <param name="httpPostedFileBase"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this HttpPostedFileBase httpPostedFileBase)
        {
            var ms = new MemoryStream();
            httpPostedFileBase.InputStream.CopyTo(ms);

            return ms.ToArray();
        }



        /// <summary>
        /// Para formatında gösterim
        /// </summary>
        /// <param name="para"></param>
        /// <param name="simge"></param>
        /// <returns></returns>
        public static string ToMoney(this decimal para, string simge = "")
        {
            return simge != "" ? $"{para:N2} {HttpUtility.HtmlDecode(simge)}" : para.ToString("N2");
        }

        /// <summary>
        /// Para formatında gösterim
        /// </summary>
        /// <param name="para"></param>
        /// <param name="simge"></param>
        /// <returns></returns>
        public static string ToMoney(this decimal? para, string simge = "")
        {

            if (para.HasValue)
            {
                return simge != "" ? $"{para.Value:N2} {HttpUtility.HtmlDecode(simge)}" : para.Value.ToString("N2");
            }

            return "";

        }

    }
}
