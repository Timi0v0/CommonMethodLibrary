using Newtonsoft.Json;
using System.Text;

namespace WebHelper
{
    public class WebApiHelper
    {
        public static async Task<TResponse?> UploadJsonToMesAsync<TRequest, TResponse>(string url, TRequest requestData, Action<string> logAction) where TResponse : class
        {
            try
            {
                // 序列化请求数据为 JSON 字符串
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                logAction?.Invoke($"MES接口: {url}");
                logAction?.Invoke($"上传MES: {jsonRequest}");
                // 使用单例 HttpClient 以减少资源消耗
                using (HttpClient client = new HttpClient())
                using (HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"))
                {
                    // 创建 HttpRequestMessage 并设置内容和头
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content })
                    {
                        // 发送异步请求
                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                        // 检查是否成功状态码
                        response.EnsureSuccessStatusCode();

                        // 读取响应内容
                        string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        logAction?.Invoke($"MES回复: {jsonResponse}");

                        // 使用委托反序列化响应数据
                        return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"上传数据到 MES 发生异常: {ex.Message}");
                return null;
            }
        }

        public static TResponse? UploadJsonToMes<TRequest, TResponse>(string url, TRequest requestData, Action<string> logAction)
        {
            try
            {
                // 序列化请求数据为 JSON 字符串
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                logAction?.Invoke($"MES接口: {url}");
                logAction?.Invoke($"上传MES: {jsonRequest}");
                // 使用单例 HttpClient 以减少资源消耗
                using (HttpClient client = new HttpClient())
                using (HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"))
                {
                    // 创建 HttpRequestMessage 并设置内容和头
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content })
                    {
                        // 发送异步请求
                        HttpResponseMessage response =  client.SendAsync(request).GetAwaiter().GetResult();

                        // 检查是否成功状态码
                        response.EnsureSuccessStatusCode();

                        // 读取响应内容
                        string jsonResponse =  response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        logAction?.Invoke($"MES回复: {jsonResponse}");

                        // 使用委托反序列化响应数据
                        return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"上传数据到 MES 发生异常: {ex.Message}");
                return default(TResponse);
            }
        }
    }
}
