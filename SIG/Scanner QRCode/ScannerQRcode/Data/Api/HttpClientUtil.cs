namespace ScannerQRcode.Data.Api
{
    public class HttpClientUtil
    {

        public static async Task<string> ConsHttpClientAsync(string endereco)
        {
            var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(endereco);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
