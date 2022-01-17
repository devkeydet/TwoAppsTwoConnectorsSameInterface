public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.Context.OperationId)
        {
            case "GetConnectorInfo" : return await this.GetConnectorInfo().ConfigureAwait(false);
            case "ListOrganizations" : return await this.ListOrganizations().ConfigureAwait(false);
            case "ListProjects" : return await this.ListProjects().ConfigureAwait(false);
            case "ListRepos" : return await this.ListRepos().ConfigureAwait(false);            
            default : return InvalidOperationId();	
        }
    }

    private async Task<HttpResponseMessage> GetConnectorInfo()
    {
        var response = new HttpResponseMessage();
        response.StatusCode = HttpStatusCode.OK;

        var newItem = new JObject
        {
            ["name"] = "GH"	
        };
        response.Content = CreateJsonContent(newItem.ToString());

        return response;
    }

    private async Task<HttpResponseMessage> ListOrganizations()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            
            var result = JArray.Parse(responseString);
            var newResult = new JArray();
            foreach (var item in result)
            {
                var newItem = new JObject
                {
                    ["name"] = item["login"]		
                };
                newResult.Add(newItem);
            }
            
            response.Content = CreateJsonContent(newResult.ToString());
        }

        return response;
    }

    private async Task<HttpResponseMessage> ListProjects()
    {
        var response = new HttpResponseMessage();
        response.StatusCode = HttpStatusCode.OK;

        // return an empty array since GitHub doesn't have projects
        var newResult = new JArray();
        var newItem = new JObject
        {
            ["name"] = ""	
        };
        newResult.Add(newItem);   
        response.Content = CreateJsonContent(newResult.ToString());

        return response;
    }

    private async Task<HttpResponseMessage> ListRepos()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            
            var result = JArray.Parse(responseString);
            var newResult = new JArray();
            foreach (var item in result)
            {
                var newItem = new JObject
                {
                    ["name"] = item["name"]		
                };
                newResult.Add(newItem);
            }
            
            response.Content = CreateJsonContent(newResult.ToString());
        }

        return response;
    }

    private HttpResponseMessage InvalidOperationId()
    {
        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }
}
