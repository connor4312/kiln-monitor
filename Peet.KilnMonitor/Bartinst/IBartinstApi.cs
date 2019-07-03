using System.Collections.Generic;
using System.Threading.Tasks;
using Peet.KilnMonitor.Contracts;

namespace Peet.KilnMonitor.Bartinst
{
    public interface IBartinstApi
    {
        /// <summary>
        /// Attempts to authenticate the given user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        Task<AuthenticateResponse> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Attempts to authenticate the given user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="authToken">User auth token</param>
        /// <returns>A list of the user's kiln status</returns>
        Task<IList<KilnStatus>> QueryAsync(string email, string authToken);
    }
}