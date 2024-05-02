using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Net.Http.Headers;
using FacilaIT.Models;

namespace FacilaIT.Business
{
    public class ProxyBusiness
    {
        public ProxyBusiness()
        {

        }
        public async Task<string> GetAsync(string url, string jwtToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Add the JWT token to the request headers
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

                    // Send the GET request
                    HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

                    // Check if the request was successful (status code 200 OK)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        // Read and return the response content as a string
                        return await httpResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Handle the error by logging or displaying the status codes
                        string messageException = $"HTTP Request Error: {httpResponse.StatusCode}";

                        // Optionally, you can handle specific status codes differently
                        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            messageException += "Resource not found.";
                        }
                        Exception e = new Exception(messageException);
                        throw e;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP request-specific exceptions
                    Exception e = new Exception($"HTTP Request Exception: {ex.Message}");
                    throw e;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw ex;
                }
            }
        }


        public async Task<string> DeleteAsync(string url, string jwtToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Add the JWT token to the request headers
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

                    // Send the GET request
                    HttpResponseMessage httpResponse = await httpClient.DeleteAsync(url);

                    // Check if the request was successful (status code 200 OK)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        // Read and return the response content as a string
                        return await httpResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Handle the error by logging or displaying the status codes
                        string messageException = $"HTTP Request Error: {httpResponse.StatusCode}";

                        // Optionally, you can handle specific status codes differently
                        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            messageException += "Resource not found.";
                        }
                        Exception e = new Exception(messageException);
                        throw e;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP request-specific exceptions
                    Exception e = new Exception($"HTTP Request Exception: {ex.Message}");
                    throw e;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw ex;
                }
            }
        }

        public async Task<string> PostAsync(string url, string payload, string jwtToken, List<CustomHeader> customHeader)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(jwtToken))
                    {
                        // Add the JWT token to the request headers
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
                    }

                    // Custom header
                    string contentType = "application/json";
                    foreach (var item in customHeader)
                    {
                        //
                        if (item.Key.ToLower() == "Content-Type".ToLower())
                        {
                            contentType = item.Value;
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(item.Value));//ACCEPT header
                        }
                        else
                        {
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                        }

                        //
                    }

                    // Create the content for the POST request
                    StringContent content = new StringContent(payload, Encoding.UTF8, contentType);

                    // Send the POST request
                    HttpResponseMessage httpResponse = null;
                    httpResponse = await httpClient.PostAsync(url, content);

                    // Check if the request was successful (status code 2xx)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        // Read and return the response content as a string
                        return await httpResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Handle the error by logging or displaying the status codes
                        string messageException = $"HTTP Request Error: {httpResponse.StatusCode}";

                        // Optionally, you can handle specific status codes differently
                        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            messageException += "Resource not found.";
                        }
                        Exception e = new Exception(messageException);
                        throw e;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP request-specific exceptions
                    Exception e = new Exception($"HTTP Request Exception: {ex.Message}");
                    throw e;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw ex;
                }
            }
        }


        public async Task<string> PutAsync(string url, string payload, string jwtToken, List<CustomHeader> customHeader)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(jwtToken))
                    {
                        // Add the JWT token to the request headers
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
                    }

                    // Custom header
                    string contentType = "application/json";
                    foreach (var item in customHeader)
                    {
                        //
                        if (item.Key.ToLower() == "Content-Type".ToLower())
                        {
                            contentType = item.Value;
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(item.Value));//ACCEPT header
                        }
                        else
                        {
                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                        }

                        //
                    }

                    // Create the content for the POST request
                    StringContent content = new StringContent(payload, Encoding.UTF8, contentType);

                    // Send the POST request
                    HttpResponseMessage httpResponse = null;
                    httpResponse = await httpClient.PutAsync(url, content);

                    // Check if the request was successful (status code 2xx)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        // Read and return the response content as a string
                        return await httpResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Handle the error by logging or displaying the status codes
                        string messageException = $"HTTP Request Error: {httpResponse.StatusCode}";

                        // Optionally, you can handle specific status codes differently
                        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            messageException += "Resource not found.";
                        }
                        Exception e = new Exception(messageException);
                        throw e;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP request-specific exceptions
                    Exception e = new Exception($"HTTP Request Exception: {ex.Message}");
                    throw e;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw ex;
                }
            }
        }


    }
}