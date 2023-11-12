using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Godot;

namespace CoreLauncher.Scripts.Systems.Fetch;

public class FetchInfo {
    public string Url;

    private Task<HttpResponseMessage> _fetchTask;

    public FetchInfo(string url) {
        Url = url;
    }
    
    public async Task<HttpResponseMessage> Fetch() {
        if (_fetchTask == null) {
            _fetchTask = Task.Run(async () => {
                long waitTime = FetchManager.GetGlobalCooldownTimeLeft() + 1000;

                if (waitTime > 0) {
                    await Task.Delay(TimeSpan.FromMilliseconds(waitTime));
                }
                
                GD.Print($"Fetch: Attempting to fetch url {Url}.");
                
                try {
                    using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient()) {
                        HttpResponseMessage response = await httpClient.GetAsync(Url);

                        if (response.StatusCode == HttpStatusCode.TooManyRequests) {
                            RetryConditionHeaderValue retryHeader = response.Headers.RetryAfter;

                            if (retryHeader == null) {
                                GD.PrintErr($"Fetch: Received a \"TooManyRequests\" header from {Url} but no retry after time was provided.");
                                return response;
                            }
                            
                            TimeSpan? timeSpan = retryHeader.Delta;

                            if (timeSpan == null) {
                                GD.PrintErr($"Fetch: Received a \"TooManyRequests\" header from {Url} but the retry after time was null.");
                                return response;
                            }
                            
                            FetchManager.GlobalCooldownEndTime = (long)Time.GetTicksMsec() + (long)timeSpan.Value.TotalMilliseconds;

                            waitTime = FetchManager.GetGlobalCooldownTimeLeft() + 1000;
                                    
                            GD.Print($"Fetch: Received a \"TooManyRequests\" header from {Url}, retrying in {waitTime} milliseconds.");
                            await Task.Delay(TimeSpan.FromMilliseconds(waitTime));

                            return await Fetch();
                        }
                        else if (response.IsSuccessStatusCode) {
                            GD.Print($"Fetch: Successfully fetched url {Url}.");
                            return response;
                        }
                        else {
                            GD.Print($"Fetch: Received un-handleable error code from {Url}: {response.StatusCode}.");
                            return response;
                        }
                    }
                }
                catch(WebException exception) {
                    GD.PrintErr($"Fetch: Error fetching url {Url}.");
                    GD.PrintErr(exception.Message);
                }

                return null;
            });
        }

        HttpResponseMessage response = await _fetchTask;
        
        FetchManager.RemoveOutgoingFetch(this);
        
        return response;
    }
}