using System;
using Autodesk.Forge;
using System.Threading.Tasks;
using v3 = Autodesk.Forge.DesignAutomation.v3;
using v3m = Autodesk.Forge.Model.DesignAutomation.v3;

namespace da_dotnetcore_lib
{
    public class da
    {
         public static async Task<string> GetAccessToken(string clientId, string clientSecret) {
            TwoLeggedApi oauth = new TwoLeggedApi();
            dynamic reply = await oauth.AuthenticateAsync(
                clientId, 
                clientSecret, 
                oAuthConstants.CLIENT_CREDENTIALS, 
                new Scope[] { Scope.CodeAll });
            string accessToken = reply.access_token;

            Console.WriteLine(accessToken);

            return accessToken;
        }

        public static async Task ListActivities(string accessToken) {
            v3.ActivitiesApi activitiesApi = new v3.ActivitiesApi();
            activitiesApi.Configuration.AccessToken = accessToken;
            v3m.PageString activities = await activitiesApi.ActivitiesGetItemsAsync();

            Console.WriteLine(" *** Activities ***");
            foreach (string activity in activities.Data)
            {
               Console.WriteLine(activity);
            }
        }

        public static async Task ListAppBundles(string accessToken) {
            v3.AppBundlesApi appbundlesApi = new v3.AppBundlesApi();
            appbundlesApi.Configuration.AccessToken = accessToken;
            v3m.PageString appbundles = await appbundlesApi.AppBundlesGetItemsAsync();

            Console.WriteLine(" *** AppBundles ***");
            foreach (string appbundle in appbundles.Data)
            {
               Console.WriteLine(appbundle);
            }
        }
    }
}
