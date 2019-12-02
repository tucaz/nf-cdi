using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NF.Util;
using HttpRequestHeaders = System.Net.Http.Headers.HttpRequestHeaders;

namespace NF.Hotmart
{
    public class Api
    {
        private static string _accessToken = null;

        private static async Task<string> AccessToken()
        {
            if (_accessToken == null) _accessToken = await GetAuthorizationToken();

            return _accessToken;
        }

        private static async Task AddBearerToken(HttpRequestHeaders headers)
        {
            var token = await AccessToken();
            headers.Add("Authorization", $"Bearer {token}");
        }

        public static async Task<List<Member>> GetAllMembers()
        {
            var transactions = await GetAllTransactions();
            return transactions.Select(transaction => transaction.Buyer)
                .DistinctBy(buyer => buyer.Email)
                .ToList();
        }

        public static async Task<List<Member>> GetActiveMembers()
        {
            var transactions = await GetAllTransactions(product: MyProduct.Product, approvedOnly: true);
            var subscriptionTransactions = await GetAllTransactions(product: MyProduct.Subscription, approvedOnly: true);

            var activeSubscriptions = await GetAllSubscriptions(activeOnly: true);
            var activeMembers = new List<Member>();

            foreach (var transaction in transactions)
            {
                var member = transaction.Buyer;
                member.IsSubscription = false;
                activeMembers.Add(member);
            }

            foreach (var subscription in activeSubscriptions)
            {

                var member = subscriptionTransactions.First(transaction =>
                    transaction.Buyer.Email == subscription.Subscriber.Email).Buyer;
                member.IsSubscription = true;
                activeMembers.Add(member);
            }

            return activeMembers;
        }

        public static async Task<List<Subscription>> GetAllSubscriptions(bool activeOnly = false,
            int rowsPerPage = 50, int page = 0)
        {
            var activeStatus = new[] {"ACTIVE"};

            var allStatuses = new[]
            {
                "ACTIVE", "DELAYED", "CANCELLED_BY_ADMIN", "CANCELLED_BY_CUSTOMER", "CANCELLED_BY_SELLER", "INACTIVE",
                "STARTED", "OVERDUE"
            };
            var totalPages = 0;
            var format = "json";
            var allPages = page == 0;
            var baseUrl = "https://api-hot-connect.hotmart.com/subscriber/rest/v2";
            var subscriptions = new List<Subscription>();
            var startDate = DateTimeOffset.Now.AddMonths(-6);
            var endDate = DateTimeOffset.Now;
            var selectedStatuses = activeOnly ? activeStatus : allStatuses;

            if (allPages)
            {
                rowsPerPage = 50;

                using (var client = new HttpClient())
                {
                    var headers = client.DefaultRequestHeaders;
                    await AddBearerToken(headers);

                    foreach (var status in selectedStatuses)
                    {
                        page = 1;
                        do
                        {
                            var url =
                                $"{baseUrl}?accessionDate={startDate.ToUnixTimeMilliseconds()}&endAccessionDate={endDate.ToUnixTimeMilliseconds()}&status={status}&page={page}&rows={rowsPerPage}";
                            var response = await client.GetAsync(url);

                            response.EnsureSuccessStatusCode();

                            var content = await response.Content.ReadAsStringAsync();

                            var data = JsonConvert.DeserializeObject<GetSubscriptionsResponse>(content,
                                new MillisecondsDateTimeConverter());
                            var totalRows = data.Size;
                            totalPages = (totalRows / rowsPerPage);
                            totalPages += rowsPerPage * totalPages < totalRows ? 1 : 0;

                            subscriptions.AddRange(data.Data);

                            page++;
                        } while (page <= totalPages);
                    }
                }
            }

            return subscriptions;
        }

        public static async Task<List<Transaction>> GetAllTransactions(int? product = null,
            bool approvedOnly = false, int rowsPerPage = 50, int page = 0)
        {
            var approvedStatuses = new[] {"APPROVED", "COMPLETE"};
            var allStatuses = new[]
            {
                "STARTED", "PROCESSING_TRANSACTION", "COMPLETE", "PRINTED_BILLET", "WAITING_PAYMENT", "APPROVED",
                "UNDER_ANALISYS", "CANCELLED", "PROTESTED", "REFUNDED", "CHARGEBACK", "BLOCKED", "OVERDUE", "PRE_ORDER"
            };
            var totalPages = 0;
            var format = "json";
            var allPages = page == 0;
            var baseUrl = "https://api-hot-connect.hotmart.com/reports/rest/v2/history";
            var transactions = new List<Transaction>();
            var startDate = DateTimeOffset.Now.AddMonths(-6);
            var endDate = DateTimeOffset.Now;
            var selectedStatuses = approvedOnly ? approvedStatuses : allStatuses;

            if (allPages)
            {
                rowsPerPage = 50;

                using (var client = new HttpClient())
                {
                    var headers = client.DefaultRequestHeaders;
                    await AddBearerToken(headers);

                    foreach (var status in selectedStatuses)
                    {
                        page = 1;
                        do
                        {
                            var url =
                                $"{baseUrl}?startDate={startDate.ToUnixTimeMilliseconds()}&endDate={endDate.ToUnixTimeMilliseconds()}&transactionStatus={status}&page={page}&rows={rowsPerPage}";

                            if (product.HasValue)
                                url += $"&productId={product}";

                            var response = await client.GetAsync(url);

                            response.EnsureSuccessStatusCode();

                            var content = await response.Content.ReadAsStringAsync();

                            var data = JsonConvert.DeserializeObject<TransactionHistoryResponse>(content,
                                new MillisecondsDateTimeConverter());
                            var totalRows = data.Size;
                            totalPages = (totalRows / rowsPerPage);
                            totalPages += rowsPerPage * totalPages < totalRows ? 1 : 0;

                            transactions.AddRange(data.Data);

                            page++;
                        } while (page <= totalPages);
                    }
                }
            }

            return transactions;
        }

        public static async Task<string> GetAuthorizationToken()
        {
            var clientId = Global.Current.Hotmart.ClientId;
            var clientSecret = Global.Current.Hotmart.ClientSecret;
            var basicAuth = Global.Current.Hotmart.BasicAuth;

            var url =
                $"https://api-sec-vlc.hotmart.com/security/oauth/token?grant_type=client_credentials&client_secret={clientSecret}&client_id={clientId}";

            using (var client = new HttpClient())
            {
                var headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", $"Basic {basicAuth}");
                var response = await client.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsAsync<JObject>();

                return content["access_token"].ToString();
            }
        }
    }
}