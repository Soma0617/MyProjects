using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace FinalExamProject.Infrastructure
{
    /// <summary>
    /// 讓 Session 可以用物件（JSON）方式存取
    /// </summary>
    public static class SessionExtensions
    {
        // Web 友善的 Json 設定（大小寫不敏感）
        private static readonly JsonSerializerOptions _opt = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>把任意物件存進 Session（序列化成 JSON 字串）</summary>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value, _opt));
        }

        /// <summary>從 Session 取出指定型別物件；沒有就回傳 default</summary>
        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return string.IsNullOrEmpty(json)
                ? default
                : JsonSerializer.Deserialize<T>(json, _opt);
        }
    }
}
