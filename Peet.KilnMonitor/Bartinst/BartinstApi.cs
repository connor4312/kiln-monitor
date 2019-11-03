namespace Peet.KilnMonitor.Bartinst
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Peet.KilnMonitor.Contracts;

    /// <summary>
    /// Implementation of the Bartinst API.
    /// </summary>
    public class BartinstApi : IBartinstApi
    {
        private const string BaseUrl = "https://www.bartinst.com";
        private readonly HttpClient httpClient = new HttpClient();

        private readonly JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        /// Attempts to authenticate the given user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        public async Task<AuthenticateResponse> AuthenticateAsync(string email, string password)
        {
            string request = JsonConvert.SerializeObject(
                new AuthenticateRequest()
                {
                    User = new AuthenticateRequestUser()
                    {
                        Email = email,
                        Password = password
                    }
                });

            try
            {
                using (HttpResponseMessage response = await this.httpClient.PostAsync(
                    $"{BartinstApi.BaseUrl}/users/login",
                    new StringContent(request, Encoding.UTF8, "application/json")))
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new ErrorResponseException(ErrorCode.InvalidCredentials, "Invalid credentials provided");
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ErrorResponseException(ErrorCode.UnknownRemoteError, await response.Content.ReadAsStringAsync());
                    }

                    using (Stream s = await response.Content.ReadAsStreamAsync())
                    using (var sr = new StreamReader(s))
                    using (var reader = new JsonTextReader(sr))
                    {
                        return this.serializer.Deserialize<AuthenticateResponse>(reader);
                    }
                }
            }
            catch (Exception e) when (!(e is ErrorResponseException))
            {
                throw new ErrorResponseException(ErrorCode.UnknownRemoteError, e.Message);
            }
        }

        /// <summary>
        /// Attempts to authenticate the given user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="authToken">User auth token</param>
        /// <returns>A list of the user's kiln status</returns>
        public async Task<IList<KilnStatus>> QueryAsync(string email, string authToken)
        {
            try
            {
                using (HttpResponseMessage response = await this.httpClient.GetAsync(
                    $"{BartinstApi.BaseUrl}/kiln_controllers.json?user_email={email}&user_token={authToken}"))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ErrorResponseException(ErrorCode.UnknownRemoteError, await response.Content.ReadAsStringAsync());
                    }

                    using (Stream s = await response.Content.ReadAsStreamAsync())
                    using (var sr = new StreamReader(s))
                    using (var reader = new JsonTextReader(sr))
                    {
                        return this.serializer.Deserialize<IList<KilnStatus>>(reader);
                    }
                }
            }
            catch (Exception e) when (!(e is ErrorResponseException))
            {
                throw new ErrorResponseException(ErrorCode.UnknownRemoteError, e.Message);
            }
        }
    }
}
